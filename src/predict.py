import torch
import torch.nn as nn
from torchvision import models, transforms
from PIL import Image
from typing import cast

def preprocess_image(image_path: str) -> torch.Tensor:
		"""
		Load and preprocess an image for model inference.

		Args:
				image_path (str): Path to the input image.

		Returns:
				Tensor: Preprocessed image tensor.
		"""
		transform = transforms.Compose([
				transforms.Resize((128, 128)),
				transforms.ToTensor(),
				transforms.Normalize(mean=[0.5, 0.5, 0.5], std=[0.5, 0.5, 0.5])
		])
		
		pil_image: Image.Image = Image.open(image_path).convert("RGB")
		tensor = cast(torch.Tensor, transform(pil_image))
		return tensor.unsqueeze(0)


def load_model(model_path: str, num_classes: int) -> nn.Module:
		"""
		Load a trained ResNet18 model from disk.

		Args:
				model_path (str): Path to the saved model weights (.pth file).
				num_classes (int): Number of output classes.

		Returns:
				model (nn.Module): The loaded and ready-to-use model.
		"""
		model = models.resnet18(weights=None)
		model.fc = nn.Linear(model.fc.in_features, num_classes)
		model.load_state_dict(torch.load(model_path, map_location="cpu"))
		model.eval()
		return model


def run(image_path: str, model_path: str, class_names: list) -> None:
		"""
		Run inference on a single image and print the predicted class.

		Args:
				image_path (str): Path to the input image.
				model_path (str): Path to the saved model.
				class_names (list): List of class names corresponding to label indices.

		Returns:
				None
		"""
		model = load_model(model_path, num_classes=len(class_names))
		image_tensor = preprocess_image(image_path)

		with torch.no_grad():
				outputs = model(image_tensor)
				probs = torch.softmax(outputs, dim=1)
				conf, predicted_idx = torch.max(probs, 1)

		predicted_label = class_names[int(predicted_idx.item())]
		print(f"Predicted: {predicted_label} ({conf.item() * 100:.2f}% confidence)")