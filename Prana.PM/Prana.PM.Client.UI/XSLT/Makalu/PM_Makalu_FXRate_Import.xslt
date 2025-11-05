<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        
        <xsl:variable name="FXRate">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>
		  
        <xsl:if test="number($FXRate) and contains(COL3,'CURNCY')">
			
          <PositionMaster>
			  
            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>
			  
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
			  
            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="substring-before(COL3,' ')"/>
            </xsl:variable>
            
            <SettlementCurrency>
               <xsl:value-of select="$PB_CURRENCY_NAME"/>
            </SettlementCurrency>
			  
            <ForexPrice>
              <xsl:value-of select="$FXRate"/>
            </ForexPrice>

            <Date>
              <xsl:value-of select="''"/>
            </Date>
			  
             <FXConversionMethodOperator> 
               <xsl:choose>
               <xsl:when test ="$PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD'">
                 <xsl:value-of select="'D'"/>
               </xsl:when>
               <xsl:otherwise>
                 <xsl:value-of select="'M'"/>
               </xsl:otherwise>
             </xsl:choose> 
             </FXConversionMethodOperator> 
			  
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>