import os
import shutil
import random
from pathlib import Path
from typing import List


def run(
    source_dir: str, target_dir: str, split_ratio: float = 0.8, seed: int = 42
) -> None:
    """
    Organizes images from a labeled folder structure into train and validation splits.

    Args:
        source_dir (str): Path to the root folder containing labeled subfolders (e.g., 'cats/', 'dogs/').
        target_dir (str): Path to the folder where 'train/' and 'val/' folders will be created.
        split_ratio (float): Fraction of images to use for training (the rest go to validation).
        seed (int): Random seed for reproducibility.
    """
    source_path = Path(source_dir)
    target_path = Path(target_dir)

    train_dir = target_path / "train"
    val_dir = target_path / "val"

    random.seed(seed)

    for label_dir in source_path.iterdir():
        if not label_dir.is_dir():
            continue

        label = label_dir.name
        images: List[Path] = list(label_dir.glob("*.jpg"))
        random.shuffle(images)

        split_idx = int(len(images) * split_ratio)
        train_images = images[:split_idx]
        val_images = images[split_idx:]

        print(">> {0} imgs for training [{1}]".format(len(train_images), label))
        print(">> {0} imgs for validation [{1}]".format(len(val_images), label))

        ensure_clean_dir(train_dir / label)
        ensure_clean_dir(val_dir / label)

        move_images(train_images, train_dir, label)
        move_images(val_images, val_dir, label)


def move_images(image_list: List[Path], dest_dir: Path, label: str) -> None:
    """
    Moves a list of image files to a destination directory under the given label.

    Args:
        image_list (List[Path]): List of image file paths to move.
        dest_dir (Path): Base destination directory (e.g., 'train/' or 'val/').
        label (str): Subfolder name under the destination (e.g., 'cats', 'dogs').
    """
    for img in image_list:
        dest_path = dest_dir / label / img.name
        shutil.copy(str(img), str(dest_path))


def ensure_clean_dir(path: Path) -> None:
    """
    Ensures a directory exists and is empty. Creates it if it doesn't exist.

    Args:
        path (Path): Path to the directory to clean or create.
    """
    if path.exists():
        for file in path.glob("*"):
            file.unlink()
    else:
        path.mkdir(parents=True, exist_ok=True)
