var bookCatalog = bookCatalog || {};

(function () {
    var self = this;

    var bookCatalogTableBody = '#BookCatalogTable tbody';
    var editBookFormBookId = '#editBookDialog #editBookForm #bookId';
    var editBookFormBookTitle = '#editBookDialog #editBookForm #title';
    var editBookFormBookYear = '#editBookDialog #editBookForm #yearBookPublished';
    var editBookFormBookPages = '#editBookDialog #editBookForm #pages';
    var editBookFormBookRate = '#editBookDialog #editBookForm #rate';
    var editBookDialog = '#editBookDialog';
    var editBookDialogForm = '#editBookDialog #editBookForm';
    var editBookDialogSaveButton = '#editBookDialog #btnSaveBookFromModal';
    var editBookDialogCloseButton = '#editBookDialog #btnCloseBookEditModal';
    var editBookMultiselectAuthors = '#editBookDialog #editBookForm #multiselectAuthors';
    var editBookMultiselectAuthorsOption = '#editBookDialog #editBookForm #multiselectAuthors option';

    var editAuthorFormAuthorId = '#editAuthorDialog #editAuthorForm #authorId'; 
    var editAuthorFormFirstName = '#editAuthorDialog #editAuthorForm #firstName';
    var editAuthorFormLastName = '#editAuthorDialog #editAuthorForm #lastName';
    var editAuthorDialog = '#editAuthorDialog';
    var editAuthorDialogForm = '#editAuthorDialog #editAuthorForm';
    var editAuthorDialogSaveButton = '#editAuthorDialog #btnSaveAuthorFromModal';
    var editAuthorDialogCloseButton = '#editAuthorDialog #btnCloseAuthorEditModal';

    var currentAdditionalBookTable = {
        rowDataId : 'td.rowDataId',
        rowDataTitle : 'td.rowDataTitle',
        rowDataPublishingYear : 'td.rowDataPublishingYear',
        rowDataPagesAmount : 'td.rowDataPagesAmount',
        rowDataRate : 'td.rowDataRate'
    };

    var bookTable;
    var bookTableEditor;
    var authorEditor;
    var urls = {
        editBookFormUrl : "ApplicationScripts/editBook.html",
        editAuthorFromUrl : "ApplicationScripts/editAuthor.html",
        expanderBookInfoUrl : "ApplicationScripts/expanderBookInfo.html"
    };
    var requiredFieldsNotFilled = "Fill all required fileds";
    var authorsArray = [];

    self.setUrlForTable = function (url) {
        urls.getBookResultTable = url.getBookResultTable;
        urls.getAuthorsList = url.getAuthorsList;
        urls.updateBook = url.updateBook;
        urls.updateAuthor = url.updateAuthor;
        urls.removeBook = url.removeBook;
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

        $(bookCatalogTableBody).on('click', 'td.details-control', function () {
            var tableRow = $(this).closest('tr');
            var tdi = tableRow.find("i.fa");
            var row = bookTable.api().row(tableRow);

            if (row.child.isShown()) {
                row.child.hide();
                tableRow.removeClass('shown');
                tdi.first().removeClass('fa-minus-square');
                tdi.first().addClass('fa-plus-square');
                self.authorsHtml = '';
            }
            else {
                getAdditionalBookInfo(tableRow);
            }
        });

        $(bookCatalogTableBody).on('click', 'td.remove-control', function () {
            var tableRow = (this).closest('tr');
            var row = bookTable.api().row(tableRow).data();
            if (confirm("Are you sure, you want to remove book: " + row.Title + "?"))
                removeBook(row.Id);
        });

        $(bookCatalogTableBody).on('click', 'td.edit-control', function () {
            var tableRow = (this).closest('tr');
            var row = bookTable.api().row(tableRow).data();
            clearBookFilledForm();

            $(editBookFormBookId).val(row.Id);
            $(editBookFormBookTitle).val(row.Title);
            $(editBookFormBookYear).datepicker().val(row.PublishingYear);
            $(editBookFormBookPages).val(row.PagesAmount);
            $(editBookFormBookRate).val(row.Rate);
            var existingAuthors = row.Authors.split(", ");
            var selectedAuthors = [];

            $.each(existingAuthors, function (i, e) {
                $(editBookMultiselectAuthorsOption).filter(function () {
                    if ($(this).text() === e) {
                        var sel = $(this).val();
                        selectedAuthors.push(sel);
                    }
                });
            });
            $(editBookMultiselectAuthors).chosen().val(selectedAuthors).trigger("chosen:updated");

            bookTableEditor.dialog("open");
        });

        bookTable.on('user-select', function (e, dt, type, cell, originalEvent) {
            if ($(cell.node()).hasClass("details-control")) {
                e.preventDefault();
            }
        });

        $('.dataTables_filter input').unbind().keyup(function (e) {
            bookTable.api().columns(2).search(this.value).draw();
        });
    };

    self.InitEditBookModal = function () {
        bookTableEditor = $(editBookDialog).load(urls.editBookFormUrl).dialog({
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
        $(editBookDialogSaveButton).click(function (e) {
            e.preventDefault();
            updateBook();
        });
        $(editBookDialogCloseButton).click(function (e) {
            clearBookFilledForm();
            $(editBookDialog).dialog("close");
        });
        $(editBookFormBookYear).datepicker({
            changeMonth: false,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'yy',
        });
    };

    self.InitEditAuthorModalButtons = function () {
        $(editAuthorDialogSaveButton).click(function (e) {
            e.preventDefault();
            updateAuthor();
        });
        $(editAuthorDialogCloseButton).click(function (e) {
            clearAuthorFilledForm();
            $(editAuthorDialog).dialog("close");
        });
    };

    self.InitEditAuthorModal = function () {
        authorEditor = $(editAuthorDialog).load(urls.editAuthorFromUrl).dialog({
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

    self.openUpdateAuthorModal = function (authorName) {
        var author = getAuthorByFullName(authorName);

        $(editAuthorFormAuthorId).val(author.id);
        $(editAuthorFormFirstName).val(author.firstName);
        $(editAuthorFormLastName).val(author.lastName);

        authorEditor.dialog("open");
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

    function removeBook(bookId) {
        if (postDataToServer({ bookId: bookId }, urls.removeBook))
            bookTable.DataTable().ajax.reload();
    };

    function updateAuthor() {
        var form = $(editAuthorDialogForm);

        if (!form.valid()) {
            alert(requiredFieldsNotFilled);
            return;
        }
        var authorForm = getAuthorFilledForm();
        if (postDataToServer(authorForm, urls.updateAuthor)) {
            clearAuthorFilledForm();
            $(editAuthorDialog).dialog("close");
        }
    }

    function postDataToServer(dataToSend, urlToSend) {
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
        var form = $(editBookDialogForm);

        if (!form.valid() || $(editBookMultiselectAuthors).selectedIndex === -1) {
            alert(requiredFieldsNotFilled);
            return;
        }
        var bookForm = getBookFilledForm();
        if (postDataToServer(bookForm, urls.updateBook)) {
            clearBookFilledForm();
            $(editBookDialog).dialog("close");
        }
    }

    function getBookFilledForm() {
        return {
            Id: $(editBookFormBookId).val(),
            title: $(editBookFormBookTitle).val(),
            publishingYear: new Date($(editBookFormBookYear).datepicker().val(), 0, 1).toJSON(),
            pagesAmount: $(editBookFormBookPages).val(),
            rate: $(editBookFormBookRate).val(),
            authors: $(editBookMultiselectAuthors).chosen().val()
        };
    }

    function clearBookFilledForm() {
        $(editBookFormBookId).val("");
        $(editBookFormBookTitle).val("");
        $(editBookFormBookYear).datepicker().val("");
        $(editBookFormBookPages).val("");
        $(editBookFormBookRate).val("");
        $(editBookMultiselectAuthors).chosen().val("").trigger("chosen:updated");
    }

    function getAuthorFilledForm() {
        return {
            id: $(editAuthorFormAuthorId).val(),
            firstName: $(editAuthorFormFirstName).val(),
            lastName: $(editAuthorFormLastName).val()
        };
    }

    function clearAuthorFilledForm() {
        $(editAuthorFormFirstName).val("");
        $(editAuthorFormLastName).val("");
    }

    function setAuthorsList(authorsList) {
        authorsArray = [];
        $.each(authorsList.authors, function (i, e) {
            authorsArray.push({
                id: e.Id,
                firstName: e.FirstName,
                lastName: e.LastName,
                fullName: e.FullName,
                amountOfBooks: e.AmountOfBooks
            });
        });

        $.each(authorsList.authors, function (i, e) {
            $('#multiselectAuthors').append($('<option>', {
                value: e.Id,
                text: e.FullName
            }))
        });
        $('.html-multi-chosen-select').chosen({ width: "100%" });
    };

    function getAmountOfBooksByAuthor(author) {
        var amount; 
        authorsArray.filter(function (elem) {
            if (elem.fullName === author)
                amount = elem.amountOfBooks;
        });
        return amount;
    };

    function getAuthorByFullName(fullName) {
        var author;
        authorsArray.filter(function (elem) {
            if (elem.fullName === fullName)
                author = elem;
        });
        return author;
    };

    function getAdditionalBookInfo(tableRowElementId) {
        var tdi = tableRowElementId.find("i.fa");
        var row = bookTable.api().row(tableRowElementId);

        $.ajax({
            type: "GET",
            url: urls.expanderBookInfoUrl,
            success: function (response) {
                response += setAuthorsForExpanderBookInfo(row.data());
                row.child(response).show();

                $(currentAdditionalBookTable.rowDataId).html(row.data().Id);
                $(currentAdditionalBookTable.rowDataTitle).html(row.data().Title);
                $(currentAdditionalBookTable.rowDataPublishingYear).html(row.data().PublishingYear);
                $(currentAdditionalBookTable.rowDataPagesAmount).html(row.data().PagesAmount);
                $(currentAdditionalBookTable.rowDataRate).html(row.data().Rate);

                tableRowElementId.addClass('shown');
                tdi.first().removeClass('fa-plus-square');
                tdi.first().addClass('fa-minus-square');

            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("Error occured during execution. Status: " + xhr.status + ", Error: " + thrownError);
            }
        });
    };

    function setAuthorsForExpanderBookInfo(rowData) {
        var existingAuthors = rowData.Authors.split(", ");
        var authorsHtml = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
        $.each(existingAuthors, function (i, e) {
            var nodeAuthorHtml = '<tr>' +
                '<td>Author:</td>' +
                '<td>' + '<i id="editAuthorButton' + i + '" class="fa fa-edit edit-author-control" onclick="bookCatalog.openUpdateAuthorModal(\'' + e + '\')"></i>' + e + ' (' + getAmountOfBooksByAuthor(e) + ' book(s))</td>' +
                '</tr>';
            authorsHtml += nodeAuthorHtml;
        });
        return authorsHtml += '</table>';
    }
}).apply(bookCatalog);
