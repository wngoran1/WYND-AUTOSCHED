// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Function to generate the calendar
function generateCalendar() {
    const today = new Date();
    const currentMonth = today.getMonth();
    const currentYear = today.getFullYear();

    const daysInMonth = new Date(currentYear, currentMonth + 1, 0).getDate();

    const calendarContainer = document.getElementById('calendarContainer');

    let calendarHTML = '<table class="calendar">';
    calendarHTML += '<tr>';
    calendarHTML += '<th>Sun</th>';
    calendarHTML += '<th>Mon</th>';
    calendarHTML += '<th>Tue</th>';
    calendarHTML += '<th>Wed</th>';
    calendarHTML += '<th>Thu</th>';
    calendarHTML += '<th>Fri</th>';
    calendarHTML += '<th>Sat</th>';
    calendarHTML += '</tr>';

    let dayOfMonth = 1;

    for (let week = 0; week < 6; week++) {
        calendarHTML += '<tr>';

        for (let dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++) {
            if ((week === 0 && dayOfWeek < new Date(currentYear, currentMonth, 1).getDay()) || (dayOfMonth > daysInMonth)) {
                calendarHTML += '<td></td>';
            } else {
                calendarHTML += '<td class="' + (dayOfMonth === today.getDate() && currentMonth === today.getMonth() && currentYear === today.getFullYear() ? 'today ' : '') + ' ' + (currentMonth === today.getMonth() ? 'current-month' : '') + '">' + dayOfMonth + '</td>';
                dayOfMonth++;
            }
        }

        calendarHTML += '</tr>';
    }

    calendarHTML += '</table>';

    calendarContainer.innerHTML = calendarHTML;
}

// Call the function to generate the calendar
generateCalendar();

function toggleMenu() {
    console.log("CLICK");
    var dropdownMenu = document.getElementById("dropdown-menu");
    dropdownMenu.classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.hamburger-icon')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        for (var i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}