import os
import shutil
from zipfile import ZipFile

# Define constants for data paths
DATA_FOLDER = "./data"  # Root directory for data
ZIP_FILENAME = "catsvsdogs-dataset.zip"  # Name of the zip file
ZIP_FILE = os.path.join(DATA_FOLDER, ZIP_FILENAME)  # Full path to the zip file
UNZIP_DIR = os.path.join(DATA_FOLDER, "archive")  # Directory to extract contents of the zip


def run():
    """
    Extracts a dataset zip file and prepares the data folder.

    This function performs the following steps:
    1. Checks if the expected zip file exists in the data directory.
    2. Deletes any existing extraction directory to avoid conflicts or stale data.
    3. Creates a new directory for extracting the contents.
    4. Unzips the dataset into the specified directory.
    5. Deletes the original zip file after successful extraction.

    Raises:
        FileNotFoundError: If the expected zip file does not exist.
    """
    
    # Check if the zip file exists; raise an error if not
    if not os.path.exists(ZIP_FILE):
        raise FileNotFoundError(f"Expected zip file not found: {ZIP_FILE}")

    # If the extraction directory already exists, remove it to start fresh
    if os.path.exists(UNZIP_DIR):
        shutil.rmtree(UNZIP_DIR)

    print("Unzipping dataset...")
    
    # Create the extraction directory
    os.makedirs(UNZIP_DIR, exist_ok=True)

    # Extract all contents of the zip file to the extraction directory
    with ZipFile(ZIP_FILE, "r") as zip_ref:
        zip_ref.extractall(UNZIP_DIR)

    # Delete the zip file after extraction to free up space
    os.remove(ZIP_FILE)
    
    print("Dataset ready at:", UNZIP_DIR)
