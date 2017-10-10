
        $(document).ready(function () {
                $('.edit').hide();
                $('.edit-case').on('click', function () {
                    var tr = $(this).parents('tr:first');
                   // var RateScaleID = tr.find('#ratescaleid').text();
                    var RateScale = tr.find('#ratescale').text();
                    var RateValue = tr.find('#ratevalue').text();
                    var Description = tr.find('#description').text();
                    //tr.find('#RateScaleID').val(RateScaleID);
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
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "http://localhost:58074/HR/RateEdit",
                        data: JSON.stringify({ "RateScaleID":RateScaleID ,"RateScale": RateScale,"RateValue": RateValue,"Description": Description }),
                        dataType: "json",
                        success: function (data) {
                            tr.find('.edit, .read').toggle();
                            $('.edit').hide();
                            tr.find('#ratescale').text(data.RateScale);
                            tr.find('#ratevalue').text(data.RateValue);
                            tr.find('#description').text(data.Description);
                            window.location = "http://localhost:58074/HR/RatingScale";
                        
                        },
                        error: function (err) {
                            alert("error");
                        }
                    });
                });
                $('.cancel-case').on('click', function (e) {
                    e.preventDefault();
                    var tr = $(this).parents('tr:first');
                    var id = $(this).prop('id');
                    tr.find('.edit, .read').toggle();
                    $('.edit').hide();
                });
                $('.delete-case').on('click', function (e) {
                    e.preventDefault();
                    var tr = $(this).parents('tr:first');
                    id = $(this).prop('id');
                    $.ajax({
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        url: "http://localhost:58074/HR/RateDelete/" + id,
                        dataType: "json",
                        success: function (data) {
                            alert('Delete success');
                            window.location.href = "http://localhost:58074/HR/RatingScale";
                        },
                        error: function () {
                            alert('Error occured during delete.');
                        }
                    });
                });
            });
   