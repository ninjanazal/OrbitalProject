from torchvision.datasets import ImageFolder
from PIL import UnidentifiedImageError

class SafeImageFolder(ImageFolder):
    """
    A subclass of torchvision.datasets.ImageFolder that handles corrupted images gracefully.

    Overrides the __getitem__ method to catch PIL.UnidentifiedImageError exceptions
    caused by corrupted image files. When a corrupted image is encountered, this class
    will print a warning, skip the corrupted image, and attempt to load the next image
    in the dataset.

    This ensures that data loading does not stop training due to corrupted images.

    Attributes:
        None (inherits all from ImageFolder)

    Methods:
        __getitem__(index):
            Attempts to load an image and its label at the given index.
            If the image is corrupted and cannot be opened, prints a warning
            and recursively tries the next index.
            Raises RuntimeError if no valid images remain to be loaded.
    """

    def __getitem__(self, index):
        """
        Retrieve the image and label at the specified index, skipping corrupted images.

        Args:
            index (int): Index of the dataset item to retrieve.

        Returns:
            tuple: (image, label) if the image is successfully loaded.

        Raises:
            RuntimeError: If no valid images remain after skipping corrupted ones.
        """
        try:
            # Try to get the image and label normally
            return super().__getitem__(index)
        except UnidentifiedImageError:
            # Handle corrupted image files by printing a warning and skipping
            print(f"Warning: Skipping corrupted image at index {index}: {self.imgs[index][0]}")

            next_index = index + 1
            if next_index < len(self.imgs):
                # Recursively try loading the next image
                return self.__getitem__(next_index)
            else:
                # No more images left to try, raise error to avoid infinite recursion
                raise RuntimeError("No valid images found after skipping corrupted ones.")