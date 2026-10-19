fn needs_store<S: tower_sessions::SessionStore>() {}
fn main() {
    needs_store::<tower_sessions_redis_store::RedisStore<tower_sessions_redis_store::fred::prelude::Pool>>();
}
