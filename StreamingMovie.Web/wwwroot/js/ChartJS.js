$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: '/Admin/TotalViewCategory', // Đường dẫn controller/action
        success: function (response) {
            console.log('Chart Data:', response);
            renderChart(response.labels, response.data);
        },
        error: function (xhr, status, error) {
            console.error("Failed to load chart data:", error);
        }
    });

    function renderChart(labels, data) {
        const ctx = document.getElementById('myDoughnutChart').getContext('2d');

        new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Views by Category',
                    data: data,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.6)',   // Red
                        'rgba(54, 162, 235, 0.6)',   // Blue
                        'rgba(255, 206, 86, 0.6)',   // Yellow
                        'rgba(75, 192, 192, 0.6)',   // Teal
                        'rgba(153, 102, 255, 0.6)',  // Purple
                        'rgba(255, 159, 64, 0.6)',   // Orange
                        'rgba(201, 203, 207, 0.6)',  // Grey
                        'rgba(255, 99, 71, 0.6)',    // Tomato
                        'rgba(60, 179, 113, 0.6)',   // MediumSeaGreen
                        'rgba(123, 104, 238, 0.6)',  // MediumSlateBlue
                        'rgba(255, 105, 180, 0.6)',  // HotPink
                        'rgba(32, 178, 170, 0.6)',   // LightSeaGreen
                        'rgba(100, 149, 237, 0.6)',  // CornflowerBlue
                        'rgba(255, 140, 0, 0.6)',    // DarkOrange
                        'rgba(147, 112, 219, 0.6)',  // MediumPurple
                        'rgba(0, 206, 209, 0.6)',    // DarkTurquoise
                        'rgba(218, 165, 32, 0.6)',   // GoldenRod
                        'rgba(244, 164, 96, 0.6)',   // SandyBrown
                        'rgba(205, 92, 92, 0.6)',    // IndianRed
                        'rgba(106, 90, 205, 0.6)'    // SlateBlue
                    ],

                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)',
                        'rgba(201, 203, 207, 1)',
                        'rgba(255, 99, 71, 1)',
                        'rgba(60, 179, 113, 1)',
                        'rgba(123, 104, 238, 1)',
                        'rgba(255, 105, 180, 1)',
                        'rgba(32, 178, 170, 1)',
                        'rgba(100, 149, 237, 1)',
                        'rgba(255, 140, 0, 1)',
                        'rgba(147, 112, 219, 1)',
                        'rgba(0, 206, 209, 1)',
                        'rgba(218, 165, 32, 1)',
                        'rgba(244, 164, 96, 1)',
                        'rgba(205, 92, 92, 1)',
                        'rgba(106, 90, 205, 1)'
                    ],

                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    }
});
