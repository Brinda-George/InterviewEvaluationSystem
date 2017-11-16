$(document).ready(function () {
    $(function () {

        //To hide the first column in webgrid that contains user id
        //hideColumn = function (column) {
        //    $('tr').each(function () {
        //        $(this).find('td,th').eq(column).hide();
        //    });
        //};
        //hideColumn(0);

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
            var flag = 0;
            if (UserName == "") {
                tr.find('#lblUserNameValidation').html('The UserName Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblUserNameValidation').empty();
            }
            var Email = tr.find("#Email").val();
            if (Email == "") {
                tr.find('#lblEmailValidation').html('The Email Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblEmailValidation').empty();
            }
            var Designation = tr.find("#Designation").val();
            if (Designation == "") {
                tr.find('#lblDesignationValidation').html('The Designation Is Required');
                flag = 1;
            }
            else {
                tr.find('#lblDesignationValidation').empty();
            }
            if (flag == 1) {
                return false;
            }

            var EmployeeId = tr.find("#lblEmployeeId").html();
            //var UserID = tr.find('#lblUserID').html();
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
            //var UserID = tr.find('#lblUserID').html();
            var UserID = $(this).prop('id');
            // var flag = confirm('You are about to delete Employee ID ' + employeeId + ' permanently.Are you sure you want to delete this record?');
            var flag = confirm('Do you want to delete the record');
            if (flag) {

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
            }
            else {
                return false;
            }
        });
        $.getJSON("HR/EfficientPaging", null, function (d) {
            $("body").append(d.Data);
            var footer = createFooter(d.Count);
            $("DataTable tfoot a").live("click", function (e) {
                e.preventDefault();
                var data = {
                    page:$(this).text()
                };
                $.getJSON("HR/EfficientPaging", data, function (html) {
                    $("DataTable").remove();
                    $("body").append(html.Data);
                    $("DataTable thead").after(footer);
                });
            });
            })
    });
});
function createFooter(d)
{
    var rowsPerPage = 5;
    var footer = "<tfoot>";
    for(i=1;i<(d+1);i++)
    {
        footer = footer + "<a href=#>" + i + "</a>&nbsp;";
    }
    footer = footer + "</tfoot>";
    $("DataTable thead").after(footer);
    return footer;
}



    
