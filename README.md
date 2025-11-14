# 🌾 WFarm - Idle Farm Management Sim

**A farm management simulation game built using Unity.**

---

## Project Overview

This project serves as a technical showcase and demo for an idle farm management game. The core gameplay loop involves players setting up fields, purchasing seeds, utilizing automated **Workers** (NPCs), and managing resources to expand the farm.

## Core Features & Technical Highlights

I focused on developing core gameplay and structure systems to ensure scalability and maintainability:

* **Inventory & Shop Management:**
    * Utilizes **Scriptable Objects** (`ItemData`, `SeedData`, etc.) for data-driven item configuration (as well as reading CSV file to change data lively).
    * Features a versatile store supporting both buying and selling of basic item types.
    * Implements **Data Mapping** logic to safely convert generic store items (`GeneralItemData`) into specialized types (`SeedData`) required by the field logic, solving common type-casting challenges.
* **Field & Planting System:**
    * Manages distinct **Field States** (`Empty`, `Growing`, `ReadyToHarvest`).
    * Implements core planting and harvesting logic tied to a central **Time Manager**.
* **Worker Automation (Finite State Machine):**
    * Workers use a **Finite State Machine (FSM)** architecture to autonomously handle farm tasks (Planting, Harvesting might be Fishing and Animal Breeding).
* **Architectural Design:**
    * Reliance on the **Singleton** pattern for core managers.
    * Using some **Event/Action** pattern to maintain low coupling between systems.

---

## ▶️ Gameplay Demonstration

For a brief, visual overview of the systems in action, please watch the gameplay demo linked below:



[VIDEO LINK](https://drive.google.com/file/d/1xTSCUl1zqhWFSVvveiS9CSLlBKKx9pMM/view?usp=drive_link)

---

## 🛠️ Technology Stack

* **Game Engine:** Unity6 (Version 54)
* **Language:** C#
* **Data Serialization:** CSV files (used for initial configuration: starting items, store inventory, game settings)

## 📌 Setup and Getting Started

1.  Clone the repository.
2.  Open the project in Unity.
3.  Load the **[Main Scene Name]** scene.
4.  Press Play.

---

**Thank you for your time and consideration!**

---
