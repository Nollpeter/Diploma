# BlazorCraft Project

## A foreword to this application

First of all, welcome to this application and thank you very much for participating in my diploma work!

---

### What this application is

This is a research project for my Master's thesis, that wishes to find answers how the learning process of a new technology can be augmented by exercises and continuous feedback, also what effect they have on the learning process.

I am very grateful to you for participating in this research, you cannot imagine how much help it is for me, if you complete this "course", and provide me with your exam results and feedback in the end!

The selected technology for this research purpose is **Microsoft Blazor WebAssembly**

This technology is a frontend framework, with which web applications can be developed using **.NET** and **C#**

---
### The aim of this application

The aim of the application is to teach you the very basics and "look and feel" of blazor, enabling you to develop individual components.

You are most certainly won't become an expert at blazor, but it can be a very good start, on which you can build on later

---
### The process of the application

The application and learning process consists of 11 chapters and an exam exercise.

These chapters will walk you through the most fundamental building blocks of blazor, which you will need to put together for the exam exercise

There are two versions of this application:

- The **Without tests** version: In this version, there are only educational materials with sample codes, after which you will have to complete the exam exercise.
    - There are some of you, that I will specifically ask to complete this version first, this is because of the research, so that I will have comparable reference data how students without the exercises perform compared to those who get exercises in the process.
    - However, if you are in this group, and interested, I welcome you to complete the version with the exercises as well, and share your experience with the two versions!

- The **With tests** version: In this version, in addition to the educational material and sample codes, you will receive exercises for the chapters (sometimes even subchapters), where you can try out what you have learned, and both during development and after passing all tests, you will be able to try out and experiment with the component that you have created.

---

Everything else that you need to know regarding the application will be described in the application (including the content above), please refer to that!

Happy coding and learning! :) 



## Prerequisites

To run this project, you will need:
- .NET 7.0 Runtime
- An IDE, Visual Studio or JetBrains Rider
- Windows 10 version 1607 (Anniversary Update) or later.
- Administrative privileges on your machine.

### Enable Long Path Support in Windows

This PowerShell script is designed to enable the long path support in Windows, allowing paths longer than the standard 260-character limit. This can be particularly useful for developers and users who work with deeply nested files and directories.

1. **Open PowerShell as Administrator**:
    - Right-click the Start button.
    - Select “Windows PowerShell (Admin)”.

2. **Run the Script**:
    - Copy the script below and paste it into the PowerShell window.
    - Press Enter to execute the script.

   ```powershell
## Running the Project

---

### Using JetBrains Rider
1. Open the solution file (`BlazorCraft.sln`) in JetBrains Rider.
2. Ensure that the `BlazorCraft.Web` project is set as the startup project.
3. Select the "BlazorCraft" launch profile from the run configuration dropdown.
4. Press `F5` or click the run button to start the application. The application will be reachable on `http://localhost:5000`
5. If you make any modifications, simply rerun the project with the same profile and refresh the page in the browser.

### Using Visual Studio
1. Open the solution file (`BlazorCraft.sln`) in Visual Studio.
2. Right-click on the `BlazorCraft.Web` project in the Solution Explorer and set it as the startup project.
3. From the toolbar, select the "BlazorCraft" profile from the run/debug configuration dropdown.
4. Press `F5` or click the run button to start debugging. The application will be reachable on `http://localhost:5000`
5. After making changes to the code, rerun the project or solution with the same profile and refresh your browser.

## License
This project is licensed under the MIT License with an additional non-commercial use clause. The project can be freely used and modified for educational, research, and personal projects, but commercial use is strictly prohibited. For more information, see the LICENSE file in this repository.
