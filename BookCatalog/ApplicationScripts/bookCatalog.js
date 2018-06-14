var bookCatalog = bookCatalog || {};

(function () {
    var self = this;
    var bookTable;
    var bookTableEditor;
    var authorEditor;
    var urls = {};
    var urlForUpdateBook = '';
    var requiredFieldsNotFilled = "Fill all required fileds";

    var authorsArray = [];

    self.setUrlForTable = function (url) {
        urls.getBookResultTable = url.getBookResultTable;
        urls.getAuthorsList = url.getAuthorsList;
        urls.updateBook = url.updateBook;
        urls.updateAuthor = url.updateAuthor;
    };

    self.InitBookTable = function () {
        bookTable = $('#BookCatalogTable').dataTable({
            "bSort": true,
            "bAutoWidth": true,
            "aaSorting": [[1, "asc"]],
            "bPaginate": true,
            "sPaginationType": "first_last_numbers",
            "iDisplayLength": 2,
            "bFilter": true,
            "bInfo": false,
            "bServerSide": true,
            "sAjaxSource": urls.getBookResultTable,
            "columnDefs": [{
                "targets": [2, 6, 7, 8],
                "orderable": false
            }],
            "processing": true,
            "language": {
                "processing": '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i>'
            },
            "aoColumns": [
                {
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": '',
                    "render": function () {
                        return '<i class="fa fa-plus-square" aria-hidden="true"></i>';
                    },
                    width: "15px"
                },
                { "data": "Id" },
                { "data": "Title" },
                { "data": "PublishingYear" },
                { "data": "PagesAmount" },
                { "data": "Rate" },
                { "data": "Authors" },
                {
                    "className": 'edit-control',
                    "render": function (data, type, row, meta) {
                        return '<i id="editBookButton" class="fa fa-edit"></i>';
                    }
                },
                {
                    "className": 'remove-control',
                    "render": function (data, type, row, meta) {
                        return '<i id="removeBookButton" class="fa fa-trash"></i>';
                    }
                }
            ],
            "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
                oSettings.jqXHR = $.ajax({
                    dataType: 'json',
                    type: "POST",
                    url: sSource,
                    data: aoData,
                    success: function (json) {
                        fnCallback(json);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert("Error occured during execution. Status: " + xhr.status + ", Error: " + thrownError);
                    }
                })
            }
        });

        $('#BookCatalogTable tbody').on('click', 'td.details-control', function () {
            var tableRow = $(this).closest('tr');
            var tdi = tableRow.find("i.fa");
            var row = bookTable.api().row(tableRow);

            if (row.child.isShown()) {
                row.child.hide();
                tableRow.removeClass('shown');
                tdi.first().removeClass('fa-minus-square');
                tdi.first().addClass('fa-plus-square');
            }
            else {
                row.child(format(row.data())).show();
                tableRow.addClass('shown');
                tdi.first().removeClass('fa-plus-square');
                tdi.first().addClass('fa-minus-square');
            }
        });

        $('#BookCatalogTable tbody').on('click', 'td.edit-control', function () {
            return '<div id="editModal" class="modal">' +
                '<div class="modal-content">' +
                '<p>Some text in modal</p>' +
                '</div></div >';
        });

        bookTable.on('user-select', function (e, dt, type, cell, originalEvent) {
            if ($(cell.node()).hasClass("details-control")) {
                e.preventDefault();
            }
        });

        // #myInput is a <input type="text"> element
        $('#searchInput').on('keyup', function () {
            bookTable.api().columns(2).search(this.value).draw();
        });


        /*
             $('#store-list').dataTable({
        "sPaginationType": "full_numbers"
    });

    $("#myFilter").on('keyup', function (){
        $('#store-list').dataTable().fnFilter(this.value);
    });
         */


        //// Event listener to the two range filtering inputs to redraw on input
        //$('#min, #max').keyup(function () {
        //    table.draw();
        //});
    };

    self.InitEditBookModal = function () {
        bookTableEditor = $('#editBookDialog').load("ApplicationScripts/editBook.html").dialog({
            autoOpen: false,
            modal: true,
            classes: {
                "ui-dialog": "modal-content",
                "ui-dialog-titlebar": "modal-header",
                "ui-dialog-title": "modal-title",
                "ui-dialog-titlebar-close": "close",
                "ui-dialog-content": "modal-body",
                "ui-dialog-buttonpane": "modal-footer"
            },
            width: 500,
            maxWidth: 500
        });
        $('#newBookBtn').click(function (e) {
            e.preventDefault();
            bookTableEditor.dialog("open");
        });
    };

    self.InitEditBookModalButtons = function () {
        $('#editBookDialog #btnSaveBookFromModal').click(function (e) {
            e.preventDefault();
            updateBook();
        });
        $('#editBookDialog #btnCloseBookEditModal').click(function (e) {
            $('#editBookDialog').dialog("close");
        });
        $('#editBookDialog #editBookForm #yearBookPublished').datepicker({
            changeMonth: false,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'yy',
        });
    };

    self.InitEditAuthorModalButtons = function () {
        $('#editAuthorDialog #btnSaveAuthorFromModal').click(function (e) {
            e.preventDefault();
            updateAuthor();
        });
        $('#editAuthorDialog #btnCloseAuthorEditModal').click(function (e) {
            $('#editAuthorDialog').dialog("close");
        });
    };

    self.InitEditAuthorModal = function () {
        authorEditor = $('#editAuthorDialog').load("ApplicationScripts/editAuthor.html").dialog({
            autoOpen: false,
            modal: true,
            classes: {
                "ui-dialog": "modal-content",
                "ui-dialog-titlebar": "modal-header",
                "ui-dialog-title": "modal-title",
                "ui-dialog-titlebar-close": "close",
                "ui-dialog-content": "modal-body",
                "ui-dialog-buttonpane": "modal-footer"
            },
            width: 500,
            maxWidth: 500
        });
        $('#newAuthorBtn').click(function (e) {
            e.preventDefault();
            authorEditor.dialog("open");
        });
    };

    //function bindAuthorsDropDown() {
    //    $.each(authorsArray, function (i, e) {
    //        $('#multiselectAuthors').append($('<option>', {
    //            value: e.value,
    //            text: e.text
    //        }));
    //    });
    //};

    function updateAuthor() {
        var form = $('#editAuthorDialog #editAuthorForm');

        if (!form.valid()) {
            alert(requiredFieldsNotFilled);
            return;
        }
        var authorForm = getAuthorFilledForm();
        if (postDataToServer(authorForm, urls.updateAuthor)) {
            clearAuthorFilledForm();
            $('#editAuthorDialog').dialog("close");
        }
    }

    function postDataToServer(dataToSend, urlToSend, ) {
        var response = true;
        $.ajax({
            url: urlToSend,
            type: "POST",
            data: dataToSend,
            success: function (json) {
                alert(json.result.ResultMessage);
                response = json.result.IsSuccessfull;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("Error occured during execution. Status: " + xhr.status + ", Error: " + thrownError);
                response = false;
            }
        });
        return response;
    }

    function updateBook() {
        var form = $('#editBookDialog #editBookForm');

        if (!form.valid()) {
            alert(requiredFieldsNotFilled);
            return;
        }
        var bookForm = getBookFilledForm();
        if (postDataToServer(bookForm, urls.updateBook)) {
            clearBookFilledForm();
            $('#editBookDialog').dialog("close");
        }
    }

    function getBookFilledForm() {
        return {
            title: $('#editBookDialog #editBookForm #title').val(),
            publishingYear: new Date($('#editBookDialog #editBookForm #yearBookPublished').datepicker().val(), 0, 1).toJSON(),
            pagesAmount: $('#editBookDialog #editBookForm #pages').val(),
            rate: $('#editBookDialog #editBookForm #rate').val(),
            authors: $('#editBookDialog #editBookForm #multiselectAuthors').chosen().val()
        };
    }

    function clearBookFilledForm() {
        $('#editBookDialog #editBookForm #title').val("");
        $('#editBookDialog #editBookForm #yearBookPublished').datepicker().val("");
        $('#editBookDialog #editBookForm #pages').val("");
        $('#editBookDialog #editBookForm #rate').val("");
        $('#editBookDialog #editBookForm #multiselectAuthors').chosen().val("").trigger("chosen:updated");;
    }

    function getAuthorFilledForm() {
        return {
            firstName: $('#editAuthorDialog #editAuthorForm #firstName').val(),
            lastName: $('#editAuthorDialog #editAuthorForm #lastName').val()
        };
    }

    function clearAuthorFilledForm() {
        $('#editAuthorDialog #editAuthorForm #firstName').val("");
        $('#editAuthorDialog #editAuthorForm #lastName').val("");
    }


    self.getAuthorsList = function () {
        $.ajax({
            type: "GET",
            url: urls.getAuthorsList,
            success: function (json) {
                setAuthorsList(json);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("Error occured during execution. Status: " + xhr.status + ", Error: " + thrownError);
            }
        });
    };

    function setAuthorsList(authorsList) {
        $.each(authorsList.authors, function (i, e) {
            $('#multiselectAuthors').append($('<option>', {
                value: e.Id,
                text: e.FullName
            }))
        });
        $('.html-multi-chosen-select').chosen({ width: "100%" });
    };

    function format(rowData) {
        return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
            '<tr>' +
            '<td>Book Id:</td>' +
            '<td>' + rowData.Id + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Title:</td>' +
            '<td>' + rowData.Title + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Year of Publishing:</td>' +
            '<td>' + rowData.PublishingYear + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Amount of Pages:</td>' +
            '<td>' + rowData.PagesAmount + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Rate:</td>' +
            '<td>' + rowData.Rate + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Authors:</td>' +
            '<td>' + rowData.Authors + '</td>' +
            '</tr>' +
            '<tr>' +
            '</table>';
    };
}).apply(bookCatalog);