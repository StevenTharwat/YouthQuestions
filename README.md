# 🙋‍♂️ Youth Questions Platform

This Blazor WebAssembly application empowers young people to ask any question freely and anonymously, while providing a structured platform for admins to manage and respond to submissions.

## 🌟 Purpose

The platform is designed with two main goals:

1. **Freedom to Ask**  
   Users can ask any question without revealing their identity, encouraging openness and honesty.

2. **Organized Admin Review**  
   Questions are stored and managed efficiently, allowing admins and speakers to review, approve, and respond to them in a clean interface.

## 🧩 Features

- 🔒 **Anonymous Submissions**  
  Users can submit questions anonymously or with a name.

- 📥 **Local Storage of Personal Questions**  
  Keeps a record of your own questions using local storage encryption.

- ✅ **Admin & SuperAdmin Panels**  
  Admins can approve, edit, or delete questions and answers. SuperAdmins can run SQL queries directly.

- ❤️ **Like System**  
  Users can “like” questions. Likes are stored locally and synced to the database.

- 📄 **PDF Export**  
  Export daily questions to a formatted PDF file.

- 💬 **Add Answers**  
  Admins can add and approve answers to questions.

## 🗂️ File Structure

- `AddQuestion.razor` / `AddQuestion.razor.cs` – Interface for users to submit questions
- `Index.razor` / `Index.razor.cs` – Main question feed with like system and navigation
- `Admin.razor` / `Admin.razor.cs` – Admin interface for approving/editing/deleting
- `SuperAdmin.razor` / `SuperAdmin.razor.cs` – Power user interface with SQL control
- `AddAnswer.razor` / `AddAnswer.razor.cs` – Add and manage answers to specific questions

## 🛠️ Technologies Used

- **Blazor WebAssembly**
- **C#**
- **Syncfusion Blazor Components**
- **LocalStorage with Base64 Encryption**
- **PDF Generator (Syncfusion.HtmlToPdfConverter)**

## 🚀 How to Run

1. Clone the repository.
2. Open the solution in Visual Studio.
3. Make sure all required Syncfusion packages are installed.
4. Set the `Startup` project to the Blazor app.
5. Run the application (`F5` or Ctrl+F5).
6. put your RegisterLicense in `program.cs`

## 🤝 Contributing

Contributions are welcome. Please open an issue or submit a pull request if you want to add a feature or improve the UI.

## 📸 Screenshots
![screenshot 1](https://github.com/StevenTharwat/YouthQuestions/blob/main/Images/1.png)
![screenshot 2](https://github.com/StevenTharwat/YouthQuestions/blob/main/Images/2.png)
![screenshot 3](https://github.com/StevenTharwat/YouthQuestions/blob/main/Images/3.png)
![screenshot 4](https://github.com/StevenTharwat/YouthQuestions/blob/main/Images/4.png)
![screenshot 5](https://github.com/StevenTharwat/YouthQuestions/blob/main/Images/5.png)
![screenshot 6](https://github.com/StevenTharwat/YouthQuestions/blob/main/Images/6.png)

