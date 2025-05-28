import os
import shutil
import random
from pathlib import Path
from typing import Tuple


def run(
		source_dir: str, target_dir: str, split_ration: float = 0.8, seed: int = 42
) -> None:
	source_path: str = Path(source_dir)
	os.makedirs(source_path, exist_ok=True)

	target_path: str = Path(target_dir)
	os.makedirs(target_path, exist_ok=True)


	train_dir: str = target_path / "train"
	val_dir = target_path / "val"

	for split_dir in [train_dir, val_dir]:
		for category in ["cats", "dogs"]:
			os.makedirs(split_dir / category, exist_ok=True)

