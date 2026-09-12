// Chart.js
document.addEventListener('DOMContentLoaded', function () {

    if (!window.dashboardData) {
        return;
    }

    initDashboardCharts();

});

function initDashboardCharts() {

    const statusCanvas = document.getElementById('statusChart');

    const priorityCanvas = document.getElementById('priorityChart');

    if (statusCanvas) {

        new Chart(statusCanvas, {
            type: 'doughnut',

            data: {
                labels: window.dashboardData.statusLabels,

                datasets: [{
                    data: window.dashboardData.statusData
                }]
            },

            options: {
                responsive: true,
                maintainAspectRatio: false,

                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    }

    if (priorityCanvas) {

        new Chart(priorityCanvas, {
            type: 'bar',

            data: {
                labels: window.dashboardData.priorityLabels,

                datasets: [{
                    label: 'Tickets',
                    data: window.dashboardData.priorityData
                }]
            },

            options: {
                responsive: true,
                maintainAspectRatio: false,

                scales: {
                    y: {
                        beginAtZero: true,

                        ticks: {
                            precision: 0
                        }
                    }
                },

                plugins: {
                    legend: {
                        display: false
                    }
                }
            }
        });
    }
}