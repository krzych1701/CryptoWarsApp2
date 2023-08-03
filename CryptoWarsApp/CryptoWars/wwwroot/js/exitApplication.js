window.Blazor = window.Blazor || {};

window.Blazor.exitApplication = function () {
    // Check if the application is running in a WebView (like in Maui) or a regular browser
    if (typeof exitApp !== 'undefined') {
        // Running in a WebView (e.g., Maui)
        exitApp();
    } else {
        // Running in a regular browser
        window.close();
    }
}
