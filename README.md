# 🖩 WPF MVVM Calculator  

## 📌 Overview  
This project is a **Windows Presentation Foundation (WPF) Calculator** that follows the **MVVM (Model-View-ViewModel)** architecture. The application provides both **Standard and Programmer Calculator** modes, allowing users to perform various arithmetic and base conversions seamlessly.  

---

## ✨ Features  
### ✅ **Core Features**  
- **MVVM Architecture**:  
  - Implements the **Model-View-ViewModel** pattern to ensure a clean separation of concerns.  
  - Uses **data binding** to connect the UI with the ViewModel efficiently.  
- **Two Calculator Modes**:  
  - **Standard Mode**: Basic arithmetic operations (+, -, *, /).  
  - **Programmer Mode**: Supports number bases (Binary, Octal, Decimal, Hexadecimal) and bitwise operations.  
- **Command Handlers**:  
  - Uses **RelayCommand** to handle UI interactions dynamically.  
  - Implements **ButtonCommandHandler, MenuCommandHandler, and BaseCommandHandler** to process inputs efficiently.  
- **Digit Grouping & Formatting**:  
  - Supports **Romanian (RO) and US (EN) formatting styles**.  
  - Uses **CultureInfo** to format numbers based on selected grouping.  
- **Memory Feature**:  
  - Allows users to store and retrieve previously calculated values.  
- **Animated UI Menu**:  
  - Implements **Storyboard-based animations** for toggling the menu.  
- **Data Persistence**:  
  - Stores user settings (last selected tab, digit grouping, and base) for future sessions.  
- **User Interaction:**
  - Supports both **keyboard input** and **mouse clicks** for all calculator functions, providing flexibility in how the user interacts with the application.

---

## 🏗 Technologies Used  
- **C# & .NET (WPF)** for application logic and UI development.  
- **MVVM Architecture** for clean and scalable code structure.  
- **XAML** for designing the graphical user interface.  
- **Data Binding & Commands** to manage UI interactions.  
- **Storyboard Animations** for a modern user experience.  

---

## 📂 Project Structure  
### **Key Command Handlers**  
1. **ButtonCommandHandler**:  
   Handles the logic for all calculator buttons (numbers, operators, and equal sign). Executes the corresponding actions when a button is pressed.

2. **BaseCommandHandler**:  
   Manages operations related to different number bases. Supports base conversions (Hexadecimal, Decimal, Binary, Octal) and ensures the correct base is selected and applied.

3. **MenuCommandHandler**:  
   Handles the **cut**, **copy**, and **paste** operations for the calculator, as well as manages **digit grouping** based on the selected format (Romanian or US).

---
  
## Learning Outcomes

Through this project, I learned how to work with the MVVM architecture in WPF, implementing data binding, commands, and ViewModel logic. I deepened my knowledge of C#, focusing on command handling, data persistence, and UI animations. Additionally, I improved my ability to manage user interactions, such as handling button clicks, menu commands (cut, copy, paste, and digit formatting), and switching between different calculator modes.

---
