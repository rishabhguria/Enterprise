<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  
  <xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
		  <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
		  <xsl:when test="contains($Number,'(')">
			<xsl:value-of select="$varNumber*-1"/>
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:value-of select="$varNumber"/>
		  </xsl:otherwise>
		</xsl:choose>
	  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
	  
	  <xsl:variable name="Cashlocal">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="translate(COL17,'+','')"/>
          </xsl:call-template>
        </xsl:variable>
		  <!--   Fund -->
		 

      <xsl:if test ="number($Cashlocal) and ((contains(COL5,'DABRA CAPITAL MASTER FUND I') or 
          contains(COL5,'RAZMAR BLUESTONE FUND LP') or contains(COL5,'ADK SOHO FUND LP') or contains(COL5,'KAZAZIAN CAPITAL MASTER FUND') or 
          contains(COL5,'IBIS SPECIAL OPP FD LP') or contains(COL5,'LEGACY WORLDWIDE INVESTMENTS') or contains(COL5,'ACG ADVISORS (UK) LLP') or 
          contains(COL5,'GSREF  L.P.')))">

			  <PositionMaster>

        <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

           <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME4" select="substring(normalize-space(COL4),1,4)"/>

            <xsl:variable name="PRANA_FUND_NAME4">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME4]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:when test ="$PRANA_FUND_NAME4!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME4"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

          <BaseCurrency>
            <xsl:value-of select="'USD'"/>
          </BaseCurrency>

			<xsl:variable name = "PB_CURRENCY_NAME" >
				<xsl:value-of select ="COL16"/>
			</xsl:variable>
		
          <LocalCurrency>
			  <xsl:value-of select ="COL16"/>
          </LocalCurrency>

          <CashValueBase>
            <xsl:value-of select="translate(COL22,'+','')"/>
          </CashValueBase>

				  <xsl:variable name ="varCashValLocal">
					  
							  <xsl:value-of select="translate(COL17,'+','')"/>
						 
				  </xsl:variable>

				  <!--<xsl:variable name ="varCashValLocal1">
					  <xsl:choose>
						  <xsl:when test ="COL19 = 'JPMORGAN DEPOSIT ACCT B       '">
							  <xsl:value-of select="number(COL20)"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="'0'"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>-->

			
						  <CashValueLocal>
							  <xsl:value-of select="translate(COL17,'+','')"/>
						  </CashValueLocal>
					 

          <Date>
            <xsl:value-of select="''"/>
          </Date>

          <PositionType>
            <xsl:value-of select="'Cash'"/>
          </PositionType>

        </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
