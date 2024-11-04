using Microsoft.Maui.ApplicationModel.Communication;
using SweetBoxApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;



namespace SweetBoxApp.Services
{
    public class SweetBoxWebApi
    {
        //מנהל תכונות מתקדמות של בקשות HTTP
        //cookies כמו תמיכה
        private HttpClient client;

        // JSON משתנה זה יכיל את ההגדרות שייקבעו בהמשך כיצד לעבד ולהמיר נתוני
        // בעת שליחת וקבלת בקשות מהשרת
        private JsonSerializerOptions jsonSerializerOptions;

        // כתובת הבסיס לכתובת השרת מותאמת לפי פלטפורמות ההרצה
        public static string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://88qq7z7g-5021.uks1.devtunnels.ms/api/" : "http://localhost:5021/api/";
        public static string ImageUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://88qq7z7g-5021.uks1.devtunnels.ms/images/" : "http://localhost:5021/images/";

        // אובייקט של מחלקת השירות שמכיל את כתובת הבסיס לשרת
        private string baseUrl;
        //מאפיין זה מחזיק את פרטי המשתמש לאחר התחברות מוצלחת.
        //ניתן להשתמש בו בכל האפליקציה לצורך בדיקה או שליפה של מידע על המשתמש המחובר
        public User LoggedInUser { get; set; }


        public SweetBoxWebApi()
        {
            // Set up the HTTP client handler to support cookies
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer(); // Initialize a container to store cookies

            // Create a new HttpClient with the handler and enable automatic disposal of the handler
            this.client = new HttpClient(handler, true);

            // Set the base URL for API requests
            this.baseUrl = BaseAddress;

            // Configure JSON serializer options to make JSON formatting more readable and case insensitive
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true, // Makes the JSON output more readable (with indents)
                PropertyNameCaseInsensitive = true // Allows deserialization to ignore property name case differences
            };
        }


        public async Task<User> Login(LoginInfo info)
        {
            // Set the URL for the specific login API function
            string url = $"{this.baseUrl}login";
            try
            {
                // Serialize the login info into a JSON string using the configured options
                string json = JsonSerializer.Serialize(info, jsonSerializerOptions);

                // Create the content to send in the POST request with proper encoding and content type
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the POST request to the server API
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();

                    User result = JsonSerializer.Deserialize<User>(resContent, jsonSerializerOptions);

                    // Store the logged-in user details for further use
                    this.LoggedInUser = result;

                    // Show a success message to the user
                    await Application.Current.MainPage.DisplayAlert("Login", "Login Succeced", "ok");
                    // Return the logged-in user details
                    return result;
                }
                else
                {
                    // ניתן להוסיף תנאים שיציגו הודעות שגיאה מתאימות לסוגים השונים
                    // טיפול בתקלות שעלולות להתרחש במהלך התחברות
                    await Application.Current.MainPage.DisplayAlert("Login", "Login Faild!", "ok");
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int?> Register(User user)
        {
            // Set the URL to point to the specific API endpoint for registering a user
            string url = $"{this.baseUrl}Register";

            try
            {
                // Serialize the User object into a JSON string to send it to the API
                string json = JsonSerializer.Serialize(user);

                // Create the content to send in the POST request with proper encoding and content type
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a POST request to the server API with the user data
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Check if the response status indicates success (HTTP status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string resContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the response to get the UserId
                    int? userId = JsonSerializer.Deserialize<int?>(resContent, jsonSerializerOptions);

                    // Return the UserId if it was received successfully
                    return userId;
                }
                else
                {
                    // Return null if the response status was not successful
                    return null;
                }
            }
            catch (Exception ex)
            {
                // If there is an exception, return null (indicating the registration failed)
                return null;
            }
        }



        public async Task<List<Sellers>> GetSellersAsync()
        {
            // The URL for the API to retrieve the list of sellers
            string url = $"{this.baseUrl}sellers";

            // Sends a GET request to the API to get the list of sellers
            HttpResponseMessage response = await client.GetAsync(url);

            // Checks if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                // Reads the response content as a JSON string
                string json = await response.Content.ReadAsStringAsync();

                // Deserializes the JSON string into a list of Sellers objects
                var sellers = JsonSerializer.Deserialize<List<Sellers>>(json, jsonSerializerOptions);

                // Returns the list of sellers
                return sellers;
            }

            // If the request fails, returns an empty list
            return new List<Sellers>();
        }


        public async Task<List<Products>> GetProductsBySellerIdAsync(int sellerId)
        {
            // Creates the URL that will be used to call the API to get products by seller ID
            string url = $"{this.baseUrl}products/{sellerId}";

            // Sends a GET request to the API to retrieve the seller's product list
            HttpResponseMessage response = await client.GetAsync(url);

            // Checks if the request was successful (status code 200)
            if (response.IsSuccessStatusCode)
            {
                // Reads the response content as a JSON string
                string json = await response.Content.ReadAsStringAsync();

                // Deserializes the JSON string into a list of Products objects
                List<Products> productList = JsonSerializer.Deserialize<List<Products>>(json, jsonSerializerOptions);

                // Returns the list of products
                return productList;
            }
            // If the request fails, returns an empty list
            return new List<Products>();
        }


        public async Task<bool> RegisterSeller(Sellers seller)
        {
            string url = $"{this.baseUrl}RegisterSeller";

            try
            {
                // Serialize the Seller object into a JSON string to send it to the API
                string json = JsonSerializer.Serialize(seller);

                // Create the content to send in the POST request with proper encoding and content type
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a POST request to the server API with the seller data
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Check if the response status indicates success (HTTP status code 200)
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Handle any errors
                return false;
            }
        }

        public async Task<Sellers> GetSellerByIdAsync(int sellerId)
        {
            // יצירת URL להורדת פרטי המוכר לפי ה-ID
            string url = $"{this.baseUrl}sellers/{sellerId}";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var seller = JsonSerializer.Deserialize<Sellers>(json, jsonSerializerOptions);
                return seller;
            }

            return null;
        
        }
        public async Task<bool> UpdateSellerAsync(Sellers updatedSeller)
        {
            // יצירת URL לעדכון פרטי המוכר לפי ה-ID
            string url = $"{this.baseUrl}sellers/{updatedSeller.SellerId}";

            try
            {
                // המרת אובייקט ה-Seller ל-JSON כדי לשלוח אותו ל-API
                string json = JsonSerializer.Serialize(updatedSeller, jsonSerializerOptions);

                // יצירת התוכן לבקשת ה-PUT עם הקידוד וסוג התוכן המתאימים
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // שליחת בקשת PUT ל-API עם הנתונים המעודכנים
                HttpResponseMessage response = await client.PutAsync(url, content);

                // בדיקה אם הבקשה הצליחה (סטטוס HTTP 200)
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות (ניתן להוסיף לוג או הודעת שגיאה למשתמש)
                return false;
            }
        }

        public async Task<bool> UpdateUserDetailsAsync(User updatedUser)
        {
            string url = $"{this.baseUrl}UpdateUserDetails";

            try
            {
                // Serialize the updated user details into JSON
                string json = JsonSerializer.Serialize(updatedUser);

                // Create the content for the PUT request
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a PUT request to the server API
                HttpResponseMessage response = await client.PutAsync(url, content);

                // Check if the request was successful
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Handle any errors
                return false;
            }
        }


    }

} 
