/*!

=========================================================
* Argon Dashboard Tailwind - v1.0.1 - Modified for ASP.NET Core
=========================================================

* Product Page: https://www.creative-tim.com/product/argon-dashboard-tailwind
* Copyright 2022 Creative Tim (https://www.creative-tim.com)

* Coded by www.creative-tim.com

=========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

*/

// Use absolute paths for ASP.NET Core
var to_build = '/assets/';

// Load perfect scrollbar if needed
if (document.querySelector('.overflow-auto')) {
  loadStylesheet(to_build + "admincss/perfect-scrollbar.css");
  loadJS(to_build + "js/perfect-scrollbar.js", true);
}

if (document.querySelector("[slider]")) {
  loadJS(to_build + "js/carousel.js", true);
}

if (document.querySelector("nav [navbar-trigger]")) {
  loadJS(to_build + "js/navbar-collapse.js", true);
}

if (document.querySelector("[data-target='tooltip']")) {
  loadJS(to_build + "js/tooltips.js", true);
  loadStylesheet(to_build + "css/tooltips.css");
}

if (document.querySelector("[nav-pills]")) {
  loadJS(to_build + "js/nav-pills.js", true);
}

if (document.querySelector("[dropdown-trigger]")) {
  loadJS(to_build + "js/dropdown.js", true);
}

if (document.querySelector("[fixed-plugin]")) {
  loadJS(to_build + "js/fixed-plugin.js", true);
}

if (document.querySelector("[navbar-main]") || document.querySelector("[navbar-profile]")) {
  if(document.querySelector("[navbar-main]")){
    loadJS(to_build + "js/navbar-sticky.js", true);
  }
  if (document.querySelector("aside")) {
    loadJS(to_build + "js/sidenav-burger.js", true);
  }
}

if (document.querySelector("canvas")) {
  loadJS(to_build + "js/charts.js", true);
}

if (document.querySelector(".github-button")) {
  loadJS("https://buttons.github.io/buttons.js", true);
}

function loadJS(FILE_URL, async) {
  // Check if file exists before loading
  let dynamicScript = document.createElement("script");

  dynamicScript.setAttribute("src", FILE_URL);
  dynamicScript.setAttribute("type", "text/javascript");
  dynamicScript.setAttribute("async", async);
  
  // Add error handler to prevent console errors
  dynamicScript.onerror = function() {
    console.warn(`Could not load JS file: ${FILE_URL}`);
  };

  document.head.appendChild(dynamicScript);
}

function loadStylesheet(FILE_URL) {
  let dynamicStylesheet = document.createElement("link");

  dynamicStylesheet.setAttribute("href", FILE_URL);
  dynamicStylesheet.setAttribute("type", "text/css");
  dynamicStylesheet.setAttribute("rel", "stylesheet");
  
  // Add error handler to prevent console errors
  dynamicStylesheet.onerror = function() {
    console.warn(`Could not load CSS file: ${FILE_URL}`);
  };

  document.head.appendChild(dynamicStylesheet);
}
