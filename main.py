import argparse


def main():
    parser = argparse.ArgumentParser(description="Image Classifier orchestrator")

    parser.add_argument("--prepare", action="store_true", help="Run data preparation")
    parser.add_argument("--train", action="store_true", help="Run model training")
    parser.add_argument("--evaluate", action="store_true", help="Run model evaluation")

    parser.add_argument(
        "--epochs", type=int, default=10, help="Number of training epochs"
    )
    parser.add_argument("--batch-size", type=int, default=32, help="Batch size")

    args = parser.parse_args()

    if args.prepare:
        print("Preparing data")
        pass


if __name__ == "__main__":
    main()
