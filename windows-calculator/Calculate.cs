using System;

namespace windows_calculator
{
    public class Calculate
    {
        public double Result = 0;
        public double MemoryNum = 0;
        public bool isFirst = true; // проверка первого числа
            
        public bool isWaiting = true; // состояние ожидания
        public bool isFocusing = false; // состояние фокусировки на числе
        public bool isBlocked = false;
        public bool OperationChanged = false; // смена знака

        public string UserInput = "";
        public string UserHistory = "";
        public string FocusNumber = "";
        public string LastOperation = "";
        public string ErrorMessage = "";

        private const int LimitAfterComma = 14;
        public void AddDigit(string ButtonContent)
        {
            if (UserInput.Length == 15 || isBlocked) return;
            UserInput += ButtonContent;
            isWaiting = false;
        }
        public void Error(string ErrorType)
        {
            switch (ErrorType) {
                case "-sqrt":
                    ErrorMessage = "недопустимый ввод";
                    break;
                case "divisionbyzero":
                    ErrorMessage = "деление на ноль невозможно";
                    break;
            }
            isBlocked = true;
        }
        public void AddComma()
        {
            if (!UserInput.Contains(",") && !UserInput.Contains("E") && !isBlocked) UserInput += ',';
        }
        public void ExecuteOperation(string ButtonContent)
        {
            if (isBlocked) return;
            if (LastOperation != ButtonContent && !isFirst && !OperationChanged) {
                OperationChanged = true;
                ExecuteOperation(LastOperation);
                isWaiting = true;
                
            }
            
            if (!isWaiting) {
                if (isFirst) {
                    Result = double.Parse(UserInput);
                    isFirst = false;
                    LastOperation = ButtonContent;
                }
                else {
                    switch (ButtonContent) {
                        case "+": 
                            Result += double.Parse(UserInput);
                            break;
                        case "-": 
                            Result -= double.Parse(UserInput);
                            break;
                        case "*": 
                            Result *= double.Parse(UserInput);
                            break;
                        case "/":
                            if (UserInput == "0") {
                                Error("divisionbyzero");
                                return;
                            }
                            Result /= double.Parse(UserInput);
                            break;
                    }
                }
            }
            else {
                if (UserHistory != "") UserHistory = UserHistory.Remove(UserHistory.Length - 1);
            }
            
            if (!OperationChanged) {
                LastOperation = ButtonContent;
                if (!isFocusing) {
                    if (UserInput != "" && UserInput[UserInput.Length - 1] == ',') UserInput = UserInput.Remove(UserInput.Length - 1);
                    UserHistory += UserInput;
                }
                UserInput = "";
                isFocusing = false;
            }
            
            UserHistory += ButtonContent;
            OperationChanged = false;
            isWaiting = true;
        }
        public void Sqrt()
        {
            if (isBlocked) return;
            if (double.Parse(UserInput) < 0) {
                Error("-sqrt");
                return;
            }
            if (!isFocusing) {
                FocusNumber = UserInput;
                UserHistory += $"sqrt({FocusNumber})";
            }
            else {
                UserHistory = UserHistory.Replace($"{FocusNumber}", $"sqrt({FocusNumber})"); 
            }
            
            FocusNumber = $"sqrt({FocusNumber})";
            UserInput = Math.Sqrt(double.Parse(UserInput)).ToString();
            
            isFocusing = true;
            isWaiting = false;
        }
        public void Negate()
        {
            if (isBlocked) return;
            if (isFocusing) {
                UserHistory = UserHistory.Replace($"{FocusNumber}", $"negate({FocusNumber})");
            }
            
            UserInput = (double.Parse(UserInput) - double.Parse(UserInput) * 2).ToString();
            isWaiting = false;
        }
        public void Percent()
        {
            if (isBlocked) return;
            if (!isFocusing) {
                UserInput = ((Result / 100) * double.Parse(UserInput)).ToString();
                UserHistory += UserInput;
            }
            else {
                FocusNumber = UserInput;
                UserInput = ((Result / 100) * double.Parse(UserInput)).ToString();
                UserHistory = UserHistory.Replace(FocusNumber, UserInput); 
            }
            isFocusing = true;
            isWaiting = false;
        }
        public void Reciproc()
        {
            if (isBlocked) return;
            if (!isFocusing) {
                FocusNumber = UserInput;
                UserHistory += $"reciproc({FocusNumber})";
            }
            else {
                UserHistory = UserHistory.Replace($"{FocusNumber}", $"reciproc({FocusNumber})"); 
            }
            
            FocusNumber = $"reciproc({FocusNumber})";
            UserInput = (1 / (double.Parse(UserInput))).ToString();
            if(Math.Abs(Math.Round(double.Parse(UserInput)) - double.Parse(UserInput)) < 1e-14) UserInput = Math.Round(double.Parse(UserInput)).ToString();

            isFocusing = true;
            isWaiting = false;
        }
        public void Backspace()
        {
            if (!isWaiting && !isBlocked) UserInput = UserInput.Remove(UserInput.Length - 1);
        }
        public void ClearInput()
        {
            if (isBlocked) return;
            UserInput = "";
        }
        public void ClearAll()
        { 
            Result = 0; 
            isFirst = true;
            isWaiting = true; 
            isFocusing = false;
            isBlocked = false;
            OperationChanged = false; 
            UserInput = "";
            UserHistory = "";
            FocusNumber = "";
            LastOperation = "";
        }
        public void Memory(string Content)
        {
            if (isBlocked) return;
            switch (Content)
            {
                case "MC":
                    MemoryNum = 0;
                    break;
                case "MR":
                    if (MemoryNum == 0) {
                        UserInput = "";
                        return;
                    }
                    UserInput = MemoryNum.ToString("0.###############");
                    isWaiting = false;
                    break;
                case "MS":
                    MemoryNum = double.Parse(UserInput);
                    break;
                case "M+":
                    MemoryNum += double.Parse(UserInput);
                    break;
                case "M-":
                    MemoryNum -= double.Parse(UserInput);
                    break;
            }
        }
        public void Equal()
        { 
            if (isBlocked) return;
            switch (LastOperation) {
                case "+": 
                    Result += double.Parse(UserInput);
                    break;
                case "-": 
                    Result -= double.Parse(UserInput);
                    break;
                case "*": 
                    Result *= double.Parse(UserInput);
                    break;
                case "/":
                    if (UserInput == "0") {
                        Error("divisionbyzero");
                        return;
                    }
                    Result /= double.Parse(UserInput);
                    break;
                case "":
                    Result = double.Parse(UserInput);
                    break;
            }
            isWaiting = false;
            isFocusing = false; 
            OperationChanged = false;
            UserInput = Result.ToString("0.###############");
            UserHistory = "";
            FocusNumber = "";
            LastOperation = "";
        }
    }
}