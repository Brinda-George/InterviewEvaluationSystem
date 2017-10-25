$(document).ready(function () {
    $(function () {

        //To hide the first column in webgrid that contains user id
        hideColumn = function (column) {
            $('tr').each(function () {
                $(this).find('td,th').eq(column).hide();
            });
        };
        hideColumn(0);

        $('.edit-mode').hide();
        $('.edit-user, .cancel-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit-mode, .display-mode').toggle();
        });

        $('.save-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            var UserName = tr.find("#UserName").val();
            var Email = tr.find("#Email").val();
            var Designation = tr.find("#Designation").val();
            var EmployeeId = tr.find("#lblEmployeeId").html();
            var UserID = tr.find('#lblUserID').html();
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
                }
            });
        });

        $('.delete-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            var UserID = tr.find('#lblUserID').html();
            $.ajax({
                url: '/HR/DeleteInterviewer/',
                data: JSON.stringify({ "UserID": UserID }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                }
            })
        });
    });
});
