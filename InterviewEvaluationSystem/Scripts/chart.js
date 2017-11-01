function getHRChart(item) {
    if (item.id == "pieChartYear") {
        $('#pieChart').attr('src', '/HR/ChartPie?year=' + item.value)
    } else if (item.id == "columnChartYear") {
        $('#columnChart').attr('src', '/HR/ChartColumn?year=' + item.value)
    }
}

function getInterviewerChart(item) {
    if (item.id == "pieChartYear") {
        $('#pieChart').attr('src', '/Interviewer/ChartPie?year=' + item.value)
    } else if (item.id == "columnChartYear") {
        $('#columnChart').attr('src', '/Interviewer/ChartColumn?year=' + item.value)
    }
}