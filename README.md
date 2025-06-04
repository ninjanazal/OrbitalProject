# OrbitalProject
# 🧠 Image Classification Study Case — Cats vs Dogs (PyTorch)

(For this project i used this [dataset](https://www.kaggle.com/api/v1/datasets/download/shaunthesheep/microsoft-catsvsdogs-dataset))


This project is a hands-on **study case** designed to explore and understand image classification using **deep learning** with **PyTorch**. The goal is not only to achieve accurate predictions but to learn how to structure, train, evaluate, and deploy a model from end to end.

---

## 📚 What This Project Teaches

This project walks through the entire deep learning workflow:

1. **Loading and transforming image datasets**
2. **Training a ResNet18 model from scratch**
3. **Evaluating performance using visual and statistical tools**
4. **Running inference on new images using Python or Jupyter**
5. **Writing clean, modular, and reusable code**

Each part of this project is meant to be instructive and transparent, emphasizing understanding over abstraction.

---

## 🗂️ Dataset Format

We use the **Cats vs Dogs** dataset (e.g., from Kaggle), organized into folders like this:


>data/
>├── train/
>│ ├── cat/
>│ └── dog/
>└── val/
>├── cat/
>└── dog/

Each subfolder represents a class label. This format is compatible with `torchvision.datasets.ImageFolder`.

---

## 🧠 Model: ResNet18

We use a **ResNet18** architecture:

- The final fully connected layer is replaced to match the number of classes (e.g., 2 for cat/dog).
- The model is trained **from scratch** to understand low-level training concepts.
- No pretrained weights are used (for learning purposes).

If desired, pretrained weights can be added easily for faster convergence.

---

## 📈 Evaluation Insights

Model evaluation is performed with:

- **Accuracy calculation**
- **Confusion matrix**
- **Classification report** (precision, recall, F1-score)
- **Visual plots** (shown in Jupyter or saved as files)

This helps identify both **what the model gets right** and **where it struggles**.

Evaluation is accessible both via script and via a Jupyter Notebook for better visualization and exploration.

---

## 🔍 Inference

You can run the model on a single image to test its performance in a real-world-like scenario.

- **Command-line script**:
  ```bash
  python main.py --predict --img=path/to/image.jpg
	```

---
## 🎓 Why This Matters
This project is meant for learners and practitioners who want:

- A practical end-to-end example of a classification pipeline
- To learn how to handle images in PyTorch
- To gain confidence in training and evaluating models
- To see both CLI and Jupyter-based workflows