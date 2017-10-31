$(document).ready(function () {
    $(function () {
        $('.edit-mode').hide();
        $('.edit-user, .cancel-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            $('.delete-user').hide();
            tr.find('.edit-mode, .display-mode').toggle();
        });

        $('.cancel-user').on('click', function () {
            $('.delete-user').show();
        });

        $('.save-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#UserName").val();
            var Email = tr.find("#Email").val();
            var Designation = tr.find("#Designation").val();
            var EmployeeId = tr.find("#lblEmployeeId").html();
            var UserID = $(this).prop('id');
            $.ajax({
                url: '/HR/UpdateInterviewer/',
                data: JSON.stringify({ "UserID": UserID, "UserName": UserName, "Email": Email, "Designation": Designation }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    tr.find('.edit-mode, .display-mode').toggle();
                    tr.find("#lblUserName").text(data.UserName);
                    tr.find("#lblDesignation").text(data.Designation);
                    tr.find("#lblEmail").text(data.Email);
                    alert("Successfully Updated");
                    $('.delete-user').show();
                }
            });
        });

        $('.delete-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            var UserID = $(this).prop('id');
            $.ajax({
                url: '/HR/DeleteInterviewer/',
                data: JSON.stringify({ "UserID": UserID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                    alert("Successfully Deleted");
                }
            })
        });
    });
});