## `pg_try_advisory_lock` and `pg_advisory_unlock` in PostgreSQL

These functions are used to manage advisory locks in PostgreSQL. Advisory locks provide a mechanism for applications to coordinate access to resources, without being tied to specific database objects. They are "advisory" because the database system doesn't enforce them; it's up to the applications to use them correctly.

### `pg_try_advisory_lock(key)`

- **Purpose**: Attempts to acquire an advisory lock.
- **Parameters**:
  - `key`: An integer or a pair of integers that uniquely identifies the lock.
- **Return Value**:
  - `true`: If the lock was successfully acquired.
  - `false`: If the lock could not be acquired because it is already held by another session.
- **Behavior**:
  - Tries to acquire the lock immediately without blocking. If the lock is available, it is acquired and `true` is returned. If the lock is already held by another session, it returns `false` immediately.
  - The lock is associated with the current database session.
  - Useful for implementing leader election, preventing concurrent execution of tasks, or coordinating access to shared resources.
- **Example**:
  ```sql
  SELECT pg_try_advisory_lock(12345);
  ```

### `pg_advisory_unlock(key)`

- **Purpose**: Releases an advisory lock.
- **Parameters**:
  - `key`: An integer or a pair of integers that identifies the lock to be released. This must match the key used when acquiring the lock.
- **Return Value**:
  - `true`: If the lock was successfully released.
  - `false`: If the lock was not held by the current session.
- **Behavior**:
  - Releases the advisory lock associated with the specified key.
  - If the lock was not held by the current session, it returns `false`.
  - It's important to release locks when they are no longer needed to avoid blocking other sessions.
- **Example**:
  ```sql
  SELECT pg_advisory_unlock(12345);
  ```

### Key Differences and Usage Notes

- **Non-Blocking vs. Blocking**: `pg_try_advisory_lock` is non-blocking, meaning it returns immediately whether it acquires the lock or not. There's also a blocking version, `pg_advisory_lock(key)`, which waits until the lock is available.
- **Session-Based**: Advisory locks are tied to the database session. When the session ends (e.g., the connection is closed), all advisory locks held by that session are automatically released.
- **No Enforcement**: PostgreSQL doesn't enforce advisory locks. It's up to the application logic to check if the lock is acquired before proceeding with the protected operation.
- **Integer Keys**: The `key` can be a single `bigint` or two `integer` values. Using two integers can be useful for creating a hierarchical locking scheme.
- **Error Handling**: Always check the return value of `pg_try_advisory_lock` to ensure the lock was acquired before proceeding with the critical section.
- **Cleanup**: Ensure that locks are always released using `pg_advisory_unlock` when they are no longer needed, especially in error scenarios.

### Example Scenario

```sql
-- Session 1:
SELECT pg_try_advisory_lock(123); -- Returns true (lock acquired)

-- Session 2:
SELECT pg_try_advisory_lock(123); -- Returns false (lock not acquired)

-- Session 1:
SELECT pg_advisory_unlock(123); -- Returns true (lock released)

-- Session 2:
SELECT pg_try_advisory_lock(123); -- Returns true (lock acquired)
```

In this scenario, Session 1 acquires the lock, preventing Session 2 from acquiring it. Once Session 1 releases the lock, Session 2 can then acquire it.