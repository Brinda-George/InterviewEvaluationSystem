$(document).ready(function () {
    $('.edit').hide();
    $(document).on('click', ".edit-case", function () {
        var tr = $(this).parents('tr:first');
        var RateScale = tr.find('#ratescale').text();
        var RateValue = tr.find('#ratevalue').text();
        var Description = tr.find('#description').text();
        tr.find('#RateScale').val(RateScale);
        tr.find('#RateValue').val(RateValue);
        tr.find('#Description').val(Description);
        tr.find('.edit, .read').toggle();
    });


    $(document).on('change', ".cat", function (e) {
        var $current = $(this);
        $current.addClass("thiss");

        $('.cat').each(function () {
            if ($(this).val() == $current.val() && $(this).attr('class') != $current.attr('class')) {
                alert('duplicate found!');
                $current.removeClass("thiss");
                $('.update-case').prop('disabled', true);
                location.reload(true);
                return false;
            }
            else {
                $('.update-case').prop('disabled', false);
            }
        });

        $current.removeClass("thiss");
    });

    $(document).on('click', ".update-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        RateScaleID = $(this).prop('id');
        var RateScale = tr.find('#RateScale').val();
        var RateValue = tr.find('#RateValue').val();
        var Description = tr.find('#Description').val();
        if (RateScale == "") {
            tr.find('#scaleLbl').html("Please enter a valid Rate Scale");
            return false;
        }
        if (RateValue == "") {
            tr.find('#valueLbl').html("Please enter a valid Rate Value");
            return false;
        }
        if (Description == "") {
            tr.find('#desLbl').html("Please enter a valid Description");
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
                alert('Successfully updated');
                window.location = data.Url;

            },
            error: function (err) {
                alert("Error occured during update.");
            }
        });
    });

    $(document).on('click', ".cancel-case", function (e) {
        e.preventDefault();
        var tr = $(this).parents('tr:first');
        var id = $(this).prop('id');
        tr.find('.edit, .read').toggle();
        $('.edit').hide();
    });

    $(document).on('click', ".delete-case", function (e) {
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
                    alert('Successfully deleted');
                    window.location.href = data.Url;
                },
                error: function () {
                    alert('Error occured during delete.');
                }
            });
        }
    });
});