$(document).ready(function () {
    $('.edit').hide();
    $('.edit-case').on('click', function () {
        var tr = $(this).parents('tr:first');
        var RateScale = tr.find('#ratescale').text();
        var RateValue = tr.find('#ratevalue').text();
        var Description = tr.find('#description').text();
        tr.find('#RateScale').val(RateScale);
        tr.find('#RateValue').val(RateValue);
        tr.find('#Description').val(Description);
        tr.find('.edit, .read').toggle();
    });

    $('.update-case').on('click', function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        RateScaleID = $(this).prop('id');
        var RateScale = tr.find('#RateScale').val();
        var RateValue = tr.find('#RateValue').val();
        var Description = tr.find('#Description').val();
        var flag = 0;
        if (RateScale == "") {
            tr.find('#scaleLbl').html("The Rate Scale field is required");
            flag = 1;
        }
        if (RateValue == "") {
            tr.find('#valueLbl').html("The Rate Value field is required");
            flag = 1;
        }
        if (Description == "") {
            tr.find('#desLbl').html("The Description field is required");
            flag = 1;
        }
        if (flag == 1) {
            return false;
        }
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/HR/RateEdit",
            data: JSON.stringify({ "RateScaleID": RateScaleID, "RateScale": RateScale, "RateValue": RateValue, "Description": Description }),
            dataType: "json",
            success: function (data) {
                tr.find('#desLbl').empty();
                tr.find('#valueLbl').empty();
                tr.find('#scaleLbl').empty();
                tr.find('.edit, .read').toggle();
                $('.edit').hide();
                tr.find('#ratescale').text(data.RateScale);
                tr.find('#ratevalue').text(data.RateValue);
                tr.find('#description').text(data.Description);
                window.location = data.Url;
            },
            error: function (err) {
                alert("Error occured during update.");
            }
        });
    });

    $('.cancel-case').on('click', function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        var id = $(this).prop('id');
        tr.find('.edit, .read').toggle();
        $('.edit').hide();
        tr.find('#desLbl').empty();
        tr.find('#valueLbl').empty();
        tr.find('#scaleLbl').empty();
    });

    $('.delete-case').on('click', function (e) {
        e.preventDefault();
        if (confirm("Are you sure you want to delete?")) {
            var tr = $(this).parents('tr:first');
            RateScaleID = $(this).prop('id');
            $.ajax({
                type: 'POST',
                url: '/HR/RateDelete/',
                data: { "RateScaleID": RateScaleID },
                dataType: "json",
                success: function (data) {
                    if (data.res == 1) {
                        alert('Successfully Deleted!!');
                    }
                    else if (data.res == 0) {
                        alert('Cannot Delete!!..Please Delete from bottom');
                    }
                    window.location.href = data.Url;
                },
                error: function () {
                    alert('Error occured during delete.');
                }
            });
        }
    });

});
