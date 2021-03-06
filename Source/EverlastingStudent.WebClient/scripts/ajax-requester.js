var app = app || {};

app.ajaxRequester = (function () {
	function ajaxRequester() {
		this.get = makeGetRequest;
		this.post = makePostRequest;
		this.put = makePutRequest;
		this.delete = makeDeleteRequest;
	}

	var makeRequest = function (url, method, data, headers) {
		var defer = Q.defer();

	    $.ajax({
	        url: url,
	        method: method,
	        contentType: 'application/x-www-form-urlencoded',
	        data: data || undefined,
	        headers: headers,
	        success: function(response) {
	            defer.resolve(response);
	        },
	        error: function(error) {
	            defer.reject(error);
	        }
	    });

		return defer.promise;
	}

	var makeGetRequest = function (url, headers) {
		return makeRequest(url, 'GET', null, headers);
	}

	var makePostRequest = function (url, data, headers) {
		return makeRequest(url, 'POST', data, headers);
	}

	var makePutRequest = function (url, data, headers) {
		return makeRequest(url, 'PUT', data, headers);
	}

	var makeDeleteRequest = function (url, headers) {
		return makeRequest(url, 'DELETE', null, headers);
	}


	return {
		get: function () {
			return new ajaxRequester();
		}
	}
}());