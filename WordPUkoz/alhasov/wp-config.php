<?php
/**
 * The base configuration for WordPress
 *
 * The wp-config.php creation script uses this file during the installation.
 * You don't have to use the website, you can copy this file to "wp-config.php"
 * and fill in the values.
 *
 * This file contains the following configurations:
 *
 * * Database settings
 * * Secret keys
 * * Database table prefix
 * * ABSPATH
 *
 * @link https://developer.wordpress.org/advanced-administration/wordpress/wp-config/
 *
 * @package WordPress
 */

// ** Database settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define( 'DB_NAME', 'wp_site_test' );

/** Database username */
define( 'DB_USER', 'root' );

/** Database password */
define( 'DB_PASSWORD', '' );

/** Database hostname */
define( 'DB_HOST', 'localhost' );

/** Database charset to use in creating database tables. */
define( 'DB_CHARSET', 'utf8' );

/** The database collate type. Don't change this if in doubt. */
define( 'DB_COLLATE', '' );

/**#@+
 * Authentication unique keys and salts.
 *
 * Change these to different unique phrases! You can generate these using
 * the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}.
 *
 * You can change these at any point in time to invalidate all existing cookies.
 * This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define( 'AUTH_KEY',         '_8eYblEM*g,}.==^q(q]=Zujf2;T7a&{^6)VDd-Qk1lWVqJtk*BL0[X [XU9(w.R' );
define( 'SECURE_AUTH_KEY',  '^:L$}Sa4@-mb)!uRp0eUs&HmbMqt;V!-lHF93Cxw8]`7f,L*QN=Z]&xHyV]U#:&<' );
define( 'LOGGED_IN_KEY',    'HG?EOLyFMyJhh7+P}Nwoz>-_<]nkk,|;sz^oCqObq_1u_E&cagzKL?U*up=R!/3q' );
define( 'NONCE_KEY',        '8g&eRH.)()%py5U5a2R&o6~|&q[jKc-cuIi@6U2gj2,plBJpqsb{|)#M,<Q-:cl5' );
define( 'AUTH_SALT',        'FABKLu$Dp=j%6_6?cMQUNrg~b:?Uj,>m{5Pua:2#`RwG#tt}(tZD&&,NiP|4e>k)' );
define( 'SECURE_AUTH_SALT', 'M&k L96wsuR?w{U,j`k8@#B/6b3Ge,58c>!uiNG%B<M0Bam|T$[wc1*27G/? zDr' );
define( 'LOGGED_IN_SALT',   '>VohNfn).466jcPuRtWR1l;_.}o,55asFzA>uf++l-/@ClR$Q3f|ge~=z8OxQ|xX' );
define( 'NONCE_SALT',       '>i{Z[O-BS%12l(B7Z#s-a7C*1x|X?s|HHUuo~V:br _g!&?TMuDO&R,>)e0,iI5$' );

/**#@-*/

/**
 * WordPress database table prefix.
 *
 * You can have multiple installations in one database if you give each
 * a unique prefix. Only numbers, letters, and underscores please!
 *
 * At the installation time, database tables are created with the specified prefix.
 * Changing this value after WordPress is installed will make your site think
 * it has not been installed.
 *
 * @link https://developer.wordpress.org/advanced-administration/wordpress/wp-config/#table-prefix
 */
$table_prefix = 'wp_';

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 *
 * For information on other constants that can be used for debugging,
 * visit the documentation.
 *
 * @link https://developer.wordpress.org/advanced-administration/debug/debug-wordpress/
 */
define( 'WP_DEBUG', false );

/* Add any custom values between this line and the "stop editing" line. */



/* That's all, stop editing! Happy publishing. */

/** Absolute path to the WordPress directory. */
if ( ! defined( 'ABSPATH' ) ) {
	define( 'ABSPATH', __DIR__ . '/' );
}

/** Sets up WordPress vars and included files. */
require_once ABSPATH . 'wp-settings.php';
