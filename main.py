import argparse
from src import *

def main():
    parser = argparse.ArgumentParser(description="Image Classifier orchestrator")

    parser.add_argument(
        "--setup", action="store_true", help="Download and extract dataset"
    )
    parser.add_argument("--prepare", action="store_true", help="Run data preparation")
    parser.add_argument("--train", action="store_true", help="Run model training")
    parser.add_argument("--evaluate", action="store_true", help="Run model evaluation")

    parser.add_argument("--batch-size", type=int, default=32, help="Batch size")

    args = parser.parse_args()

    if args.setup:
        print(">> Fetching the data")
        setup.run()

    if args.prepare:
        print(">> Preparing data")
        prepare.run("data/archive/PetImages", "data")

    if args.train:
        train.run("data/train", "data/val")
        print(">> Training model")


if __name__ == "__main__":
    main()
