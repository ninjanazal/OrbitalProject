.PHONY: all download unzip clean

DATA_FOLDER:=./data
ZIP_FILE=$(DATA_FOLDER)/microsoft-catsvsdogs-dataset.zip
DATASET_URL := https://www.kaggle.com/api/v1/datasets/download/shaunthesheep/microsoft-catsvsdogs-dataset
UNZIP_DIR=$(DATA_FOLDER)/archive

fetch_data:
	rm -f $(ZIP_FILE)
	rm -rf $(UNZIP_DIR)

	@echo "Fetching dataset..."
	curl -L -o $(ZIP_FILE) $(DATASET_URL)
	
	@echo "Unzipping dataset..."
	mkdir -p $(UNZIP_DIR)
	unzip -q -o $(ZIP_FILE) -d $(UNZIP_DIR)

		rm -f $(ZIP_FILE)