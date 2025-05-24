# 🎮 Element Rift - 3D Multiplayer Arena Game (Unity)

**Element Rift** is a first-person multiplayer arena combat game developed using Unity as part of an undergraduate capstone project in Information Technology.  
This project offers hands-on experience in game development, distributed backend systems, and cross-platform game deployment.

---

## 🧠 Game Overview

Players are thrown into an arena where they must fight opponents, explore the map, and collect key fragments from treasure chests.  
Victory can be achieved either by defeating the opponent or collecting enough key fragments to unlock the central gate.

---

## 🧩 Key Features

### ⚔️ Class-Based Character System
- Multiple character classes: **Swordsman**, **Mage**, **Brawler**, **Summoner**
- Each class has unique abilities, combat animations, and fighting styles

### 🌟 Elemental System
- Characters are assigned elements: **Fire**, **Wood**, **Darkness**, **Light**
- Elements affect skill unlocks and provide advantages during combat

### 🗺️ Exploration & Missions
- Key fragments are hidden in treasure chests across the map
- Collecting all required fragments allows the player to open the central gate

### 🏆 Victory Conditions
- Defeat the opponent
- Or open the central gate before the other player

### 💻 Cross-Platform Support
- Available on **PC (Windows & Linux)**
- Utilizes Unity's cross-platform build system

---

## ⚙️ Technologies Used

| Component                 | Technology             |
|---------------------------|------------------------|
| Game Engine               | Unity 3D               |
| Programming Language      | C#                     |
| Backend                   | Spring Boot (Java)     |
| Database (Optional)       | PostgreSQL             |
| Game-Backend Communication| REST API               |

---

## 🔐 User Authentication System

- Built with **Spring Boot**
- Unity connects to the backend to:
  - Authenticate users
  - Save match results
  - Store player stats, unlocked skills, and attributes
