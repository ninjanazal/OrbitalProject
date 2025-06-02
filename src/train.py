import torch
import torch.nn as nn
import torch.optim as optim
from torchvision import datasets, transforms, models
from torch.utils.data import DataLoader
from pathlib import Path
from .tools import SafeImageFolder


def run(
    train_dir: str,
    val_dir: str,
    model_name: str,
    num_epochs: int = 5,
    batch_size: int = 32,
    lr: float = 0.001,
    print_every: int = 10,
) -> None:
    """
    Train a CNN model (ResNet18) to classify images in two classes (e.g., cats vs dogs).

    Args:
        train_dir (str): Path to the training data directory. Must have subfolders per class.
        val_dir (str): Path to the validation data directory. Must have subfolders per class.
        num_epochs (int): Number of epochs to train for. Default is 5.
        batch_size (int): Number of samples per batch. Default is 32.
        lr (float): Learning rate for the Adam optimizer. Default is 0.001.
        print_every (int): Print training progress every this many batches.

    Returns:
        None. Trains the model and saves the trained weights to 'model.pth'.
    """
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    print(f"Using device: {device}")

    transform = transforms.Compose(
        [
            transforms.Resize((128, 128)),
            transforms.ToTensor(),
            transforms.Normalize(mean=[0.5], std=[0.5]),
        ]
    )

    train_dataset = SafeImageFolder(train_dir, transform=transform)
    val_dataset = SafeImageFolder(val_dir, transform=transform)

    train_loader = DataLoader(train_dataset, batch_size=batch_size, shuffle=True)
    val_loader = DataLoader(val_dataset, batch_size=batch_size)

    # === Data Analysis Outputs ===
    print("\n===== Dataset Information =====")
    print(f"Classes: {train_dataset.classes}")
    print(f"Number of classes: {len(train_dataset.classes)}")

    train_targets = [label for _, label in train_dataset.imgs]
    val_targets = [label for _, label in val_dataset.imgs]

    from collections import Counter

    train_counts = Counter(train_targets)
    val_counts = Counter(val_targets)

    print("Training set samples per class:")
    for idx, class_name in enumerate(train_dataset.classes):
        print(f"  {class_name}: {train_counts.get(idx,0)} images")
    print(f"Total training images: {len(train_dataset)}")

    print("Validation set samples per class:")
    for idx, class_name in enumerate(val_dataset.classes):
        print(f"  {class_name}: {val_counts.get(idx,0)} images")
    print(f"Total validation images: {len(val_dataset)}")

    sample_images, sample_labels = next(iter(train_loader))
    print(f"\nSample batch shape (images): {sample_images.shape}")
    print(f"Sample batch shape (labels): {sample_labels.shape}")
    print("===============================\n")

    model = models.resnet18(weights=None)
    model.fc = nn.Linear(model.fc.in_features, len(train_dataset.classes))
    model = model.to(device)

    criterion = nn.CrossEntropyLoss()
    optimizer = optim.Adam(model.parameters(), lr=lr)

    for epoch in range(num_epochs):
        model.train()
        running_loss = 0.0
        correct = 0

        for batch_idx, (images, labels) in enumerate(train_loader, 1):
            images, labels = images.to(device), labels.to(device)

            optimizer.zero_grad()
            outputs = model(images)
            loss = criterion(outputs, labels)
            loss.backward()
            optimizer.step()

            running_loss += loss.item()
            correct += (outputs.argmax(1) == labels).sum().item()

            # Print progress every 'print_every' batches
            if batch_idx % print_every == 0 or batch_idx == len(train_loader):
                avg_loss = running_loss / batch_idx
                accuracy_so_far = correct / (batch_idx * batch_size)
                print(
                    f"Epoch {epoch + 1} [{batch_idx}/{len(train_loader)}], "
                    f"Avg Loss: {avg_loss:.4f}, Accuracy so far: {accuracy_so_far:.4f}"
                )

        epoch_accuracy = correct / len(train_dataset)
        print(
            f"Epoch {epoch + 1} completed. Loss: {running_loss/len(train_loader):.4f}, "
            f"Accuracy: {epoch_accuracy:.4f}\n"
        )

    model_path = Path(model_name)
    torch.save(model.state_dict(), model_path)
    print(f"Model saved to {model_path.resolve()}")