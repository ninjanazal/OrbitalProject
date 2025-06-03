import argparse
from src import *
from pathlib import Path


MODEL_NAME: str = "catvsdog_model.pth"


def main():
    parser = argparse.ArgumentParser(description="Image Classifier orchestrator")

    parser.add_argument(
        "--setup", action="store_true", help="Download and extract dataset"
    )
    parser.add_argument("--prepare", action="store_true", help="Run data preparation")
    parser.add_argument("--train", action="store_true", help="Run model training")
    parser.add_argument("--evaluate", action="store_true", help="Run model evaluation")
    parser.add_argument("--predict", action="store_true", help="Run model prediction")
    parser.add_argument("--img", type=str,help="Path to the input image")

    parser.add_argument("--batch-size", type=int, default=32, help="Batch size")

    args = parser.parse_args()

    if args.setup:
        print(">> Fetching the data")
        setup.run()

    if args.prepare:
        print(">> Preparing data")
        prepare.run("data/archive/PetImages", "data")

    if args.train:
        train.run("data/train", "data/val", MODEL_NAME)
        print(">> Training model")

    if args.evaluate:
        evaluate.run("data/val", MODEL_NAME)
        print(">> Evaluating model")


    if args.predict:
        if not args.img:
            print("❌ Error: --img argument is required.")
            return

        image_path = Path(args.img)
        if not image_path.is_file():
            print(f"Error: Image file '{image_path}' not found.")
            return

        predict.run(str(image_path), MODEL_NAME, ["cat", "dog"])

if __name__ == "__main__":
    main()
