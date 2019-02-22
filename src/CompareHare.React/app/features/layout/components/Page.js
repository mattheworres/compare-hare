import React from 'react';
import PropTypes from 'prop-types';

export default function Page({className, loading, children}) {
  return (
    <div className={`page ${className || ''}`}>
      {loading && (
        <div className="loading-overlay">
          <div className="loading-indicator-container">
            <div className="lds-ring">
              <div />
              <div />
              <div />
              <div />
            </div>
          </div>
        </div>
      )}
      {!loading && children}
    </div>
  );
}

Page.propTypes = {
  className: PropTypes.string,
  loading: PropTypes.bool,
  children: PropTypes.oneOfType([
    PropTypes.node,
    PropTypes.arrayOf(PropTypes.node),
  ]),
};
