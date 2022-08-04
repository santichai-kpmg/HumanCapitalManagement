function ObjChart1(objItem, sLoactionName, sDate) {
    //if (objItem != undefined) {
    var option = {
        data: {
            labels: objItem.sLabel,
            datasets: [{
                data: objItem.Data,
                backgroundColor: [
                    "#2ecc71",
                    "#3498db",
                    "#95a5a6",
                    "#9b59b6",
                    "#f1c40f",
                    "#e74c3c",
                    "#34495e"
                ],
            }],
        },
        ChartName: "สกุลเงินสูงสุด 5 อันดับ ประจำเดือน " + sDate + " สาขา " + sLoactionName,
    };
    var ctx = document.getElementById("canvas").getContext("2d");
    BindChart(option, ctx)
    //}

}
function BindChart(option, ctx) {

    pChart = new Chart(ctx, {
        type: 'pie',
        data: option.data,
        options: {
            responsive: true,
            title: {
                display: true,
                text: option.ChartName,
                fontSize: 20,
            },
            legend: {
                position: 'right'
            },
            maintainAspectRatio: false,
            pieceLabel: {
                render: 'percentage',
                fontColor: 'white',
                precision: 2,
                fontSize: 18,
            }
        }
    });
}

function resetCanvas() {
    if (pChart != undefined) {
        pChart.destroy();
    }
};
