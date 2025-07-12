# 🧩 Simple Plugin for nopCommerce v4.80+

## Description

This plugin adds a configurable **Welcome Message** to the bottom of the **Contact Us** page in a nopCommerce store using a widget zone. The message is managed from the admin panel and can be toggled on/off without modifying the core nopCommerce code.

---

## 📦 Plugin Components

### 1. `SimplePlugin.cs`
- Inherits from `BasePlugin` and implements `IWidgetPlugin`.
- Defines configuration page and registers widget zone (`contactus_bottom`).

### 2. `SimplePluginSettings.cs`
- Holds admin-configurable settings:
  - `EnableFeature`: Toggle message on/off.
  - `WelcomeMessage`: Custom message text.

### 3. `SimplePluginModel.cs`
- Used in the configuration UI for managing plugin settings.

### 4. `SimplePluginController.cs`
- Admin controller to display and process plugin configuration form.

### 5. `Configure.cshtml`
- Razor view for admin configuration page.

### 6. `SimplePluginMessageViewComponent.cs`
- Displays the configured message in the frontend using a widget zone.

### 7. `PublicInfo.cshtml`
- Razor view used by the widget to render the welcome message on the Contact Us page.

---

## 🛠 Installation & Usage

1. **Build the Plugin**  
   Ensure the plugin is placed in the `Plugins` folder and rebuild the nopCommerce solution.

2. **Install the Plugin**  
   In nopCommerce Admin Panel:  
   - Go to **Configuration → Local Plugins**  
   - Locate **Simple Plugin** and click **Install**

3. **Enable the Widget**  
   In Admin:  
   - Go to **Configuration → Widgets**  
   - Find **Simple Plugin Message** and mark it as **Enabled**

4. **Configure Message**  
   After installation, click **Configure**  
   - Set your welcome message  
   - Toggle "Enable Feature" on or off  
   - Click **Save**

5. **Check the Contact Us Page**  
   The message will appear at the **bottom** of the Contact Us form (beneath the default content), styled for clear visibility.

---

## 💡 Notes

- This plugin uses a nopCommerce widget zone to inject content dynamically.
- No changes are required in the core nopCommerce code.
- You can extend this plugin to show different messages on other pages by adding more widget zones.

---

👩‍💻 Author: Susmy  
📘 Compatible with nopCommerce v4.8.8  
📦 Group: Misc  
🔧 SystemName: `Misc.SimplePlugin`
