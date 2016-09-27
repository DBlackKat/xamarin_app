# xamarin_app

###學聯會資訊部——第一個app

####最低需要Android 2.2 - Level 8
####支援目標Android 5 Level 22

##設立Xamarin教學 | Setup Xamarin Guide
https://developer.xamarin.com/guides/android/getting_started/installation/windows/manual_installation/

##關於Xamarin更多教學
Xamarin教學列表：http://no2don.blogspot.com/search/label/Xamarin
List Fragement 實作: http://no2don.blogspot.com/2016/07/xamarin-listfragment-list.html
Navbar 實作： http://no2don.blogspot.com/2016/07/xamarin-android-navigationdrawer_7.html

```
  關於當麻許的教學：先前有跟著作者的教學實作過幾個例子，覺得教學文寫的不是很好。但示範了Xamarin能實作的功能到什麼程度。而且作者的手機版本有點舊，所以設計都很復古
```

| First Header  | Second Header |
| ------------- | ------------- |
| ![Alt text](/readme_images/directory.png?raw=true "File directory") |
  MainActivity.cs -> APP首頁，登入端
  從這裡點擊任何一個button便開啟該頁面的Activity
  每個Activity的設計介面在／layout 底下
      1. Main.axml : 開啟app最先進入的頁面
      2. Search.axml : 查詢繳費用戶的地方
      3. student.axml : 新增繳費學生的頁面（ 只能更新local資料庫，不能更新api）
|
    * 畫面切換可以想成是一個activity，每當切換頁面，便將新的畫面的activity 跑起來。而每個activity的最上方會最先load出頁面UI
  ```
    public class InputNewUser : Activity
    {
      protected override void OnCreate(Bundle savedInstanceState)
      {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.student); /*    load ui page   */
        var sqlLiteFilePath = GetFileStreamPath("") + "/db_user.db";
        var name = FindViewById<EditText>(Resource.Id.studentName);
        var stuID = FindViewById<EditText>(Resource.Id.studentID);
  ```
    * 整個APP的核心在MainActivity.cs中，所以大家請先看過


  ![Alt text](/readme_images/app_ui.jpg?raw=true "App UI")
