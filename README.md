# 💬 Asynchronous TCP Chat System (WinForms & C#)

This project was developed as part of the **Object-Oriented Programming (OOP)** course at **Fırat University, Software Engineering Department**. The application is a chat platform featuring a Server-Client architecture running on an asynchronous structure.

## 🎓 Student Information
* **Name & Surname:** Esra Gürbüz
* **Student ID:** 230543001
* **University:** Fırat University
* **Department:** Software Engineering (Sophomore / 2nd Year)

## 🚀 Project Features
* **Asynchronous Architecture:** Utilizes `async/await` patterns to ensure a responsive, non-blocking UI.
* **TCP/IP Protocol:** Reliable data transmission implemented via `TcpListener` and `TcpClient` classes.
* **JSON Serialization:** Messages are transmitted across the network in JSON format using the `Newtonsoft.Json` library.
* **OOP Principles Applied:**
    * **Encapsulation:** Message models are protected using properties with controlled access.
    * **Inheritance:** Implemented a robust messaging hierarchy with a `MessageBase` parent class and `ChatMessage` derived class.
    * **Abstraction:** Core communication logic and data models are abstracted into a Shared Library for reusability.

## 🛠️ Tech Stack
* **Language:** C# (.NET Core / .NET Framework)
* **User Interface:** Windows Forms (WinForms)
* **Dependencies:** Newtonsoft.Json (NuGet Package)
* **IDE:** Visual Studio 2022

## 📁 Project Structure
1. **ChatApp.Server:** A console application that listens for connections and broadcasts incoming messages to all active clients.
2. **ChatApp.Client:** A desktop application with a responsive UI, featuring panel-based state management (Login vs. Chat modes).
3. **ChatApp.Shared:** A Class Library containing shared data models and common utility logic.


## 🔧 Installation & Setup
1. Clone this repository: `git clone [your-repo-url-here]`
2. Open the `.sln` file in Visual Studio.
3. Right-click on the **Solution** -> **Set Startup Projects**.
4. Select **Multiple Startup Projects** and set both **Server** and **Client** to "Start".
5. Run the application (F5).

---
*Developed for educational purposes.*
