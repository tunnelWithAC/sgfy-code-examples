import http from 'k6/http';
import { check } from 'k6';
import fs from 'fs';
import parse from 'csv-parse';

export default function () {
    // fs.readFile('./oktaCredentia', function (err, fileData) {
    //     parse(fileData, {columns: false, trim: true}, function(err, rows) {
    //         // Your CSV data is in an array of arrys passed to this callback as rows.
    //     })
    // })

    var url = 'https://{yourOktaDomain}/oauth2/default/v1/token';

    const oktaUserId = process.env.OKTA_USER_ID;
    const oktaPassword = process.env.OKTA_PASSWORD;
    var payload = JSON.stringify({
        grant_type: 'password',
        username: oktaUserId,
        password: oktaPassword,
        scope: 'openid',
    });
    var params = {
    headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        'Accept': 'application/json',
        'Authorization': 'Basic ' + btoa('{clientId}:{clientSecret}'),
    },
    };

    let res = http.post(url, payload, params);

    check(res, {
    'status was 200': (r) => r.status == 200,
    'token is present': (r) => r.json().access_token !== '',
    });

    let token = res.json().access_token;

    // Use the token in subsequent requests
    let authParams = {
    headers: {
        'Authorization': 'Bearer ' + token,
    },
    };

    let response = http.get('http://test.k6.io', authParams);
}