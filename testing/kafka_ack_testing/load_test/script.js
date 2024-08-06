import http from 'k6/http';
import { check, fail } from 'k6';

export const options = {
  // A number specifying the number of VUs to run concurrently.
  vus: 1,
  // A string specifying the total duration of the test run.
  duration: '2s',
}

function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

// Usage
async function demo() {
  console.log('Taking a break...');
  await sleep(2000);
  console.log('Two seconds later');
}

// demo();
export default function () {
  http.get('http://localhost:1080/somePathOne');
  sleep(1000);

  const res = http.get('http://localhost:3000/api');
  console.log(res.body);

  fail('unexpected response');

//  `` const checkOutput = check(
//     res,
//     {
//       'response code was 200': (res) => res.status == 200,
//     //   'body size was 1234 bytes': (res) => res.body.length == 1234,
//     },
//     { myTag: "I'm a tag" }
//   );

//   if (!checkOutput) {
//     fail('unexpected response');
//   }
}
