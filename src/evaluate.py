import matplotlib
matplotlib.use('Agg')  # Use headless backend for matplotlib

import torch
import torch.nn as nn
from torchvision import datasets, transforms, models
from torch.utils.data import DataLoader
from sklearn.metrics import classification_report, confusion_matrix
import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
from pathlib import Path
from .tools import SafeImageFolder


def run(val_dir: str, model_path: str, batch_size: int = 32) -> None:
    """
    Evaluate a trained PyTorch model on a validation dataset.

    Args:
        val_dir (str): Path to the validation data directory.
        model_path (str): Path to the trained model (.pth file).
        batch_size (int): Batch size for evaluation. Default is 32.

    Returns:
        None. Prints evaluation metrics and saves confusion matrix plot to file.
    """
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    print(f"Using device: {device}")

    # Data transforms
    transform = transforms.Compose([
        transforms.Resize((128, 128)),
        transforms.ToTensor(),
        transforms.Normalize(mean=[0.5], std=[0.5])
    ])

    # Load validation data
    val_dataset = SafeImageFolder(val_dir, transform=transform)
    val_loader = DataLoader(val_dataset, batch_size=batch_size, shuffle=False)

    # Load model
    model = models.resnet18(weights=None)
    model.fc = nn.Linear(model.fc.in_features, len(val_dataset.classes))
    model.load_state_dict(torch.load(model_path, map_location=device))
    model.to(device)
    model.eval()

    print("Model loaded and ready for evaluation.")

    # Store all predictions and labels
    all_preds = []
    all_labels = []

    with torch.no_grad():
        for images, labels in val_loader:
            images, labels = images.to(device), labels.to(device)
            outputs = model(images)
            _, preds = torch.max(outputs, 1)
            all_preds.extend(preds.cpu().numpy())
            all_labels.extend(labels.cpu().numpy())

    # Classification report
    print("\n=== Classification Report ===")
    report = classification_report(all_labels, all_preds, target_names=val_dataset.classes)
    print(report)

    # Confusion matrix
    cm = confusion_matrix(all_labels, all_preds)
    plt.figure(figsize=(6, 5))
    sns.heatmap(cm, annot=True, fmt="d", cmap="Blues",
                xticklabels=val_dataset.classes,
                yticklabels=val_dataset.classes)
    plt.xlabel("Predicted")
    plt.ylabel("Actual")
    plt.title("Confusion Matrix")
    plt.tight_layout()

    # Save to file
    output_path = Path("confusion_matrix.png")
    plt.savefig(output_path)
    print(f"\nConfusion matrix saved to {output_path.resolve()}")
