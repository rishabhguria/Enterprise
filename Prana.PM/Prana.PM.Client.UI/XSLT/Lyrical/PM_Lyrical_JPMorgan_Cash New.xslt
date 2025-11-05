<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
		  <!--   Fund -->
		  <xsl:variable name = "PB_FUND_NAME" >
			  <xsl:value-of select="substring(COL2,1,8)"/>
		  </xsl:variable>

		  <xsl:variable name="PRANA_FUND_NAME">
			  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='JPM']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
		  </xsl:variable>

		  <xsl:if test ="(COL4 = 'A' or (COL4 = 'B' and (COL19 = 'JPMORGAN DEPOSIT ACCT B       ' or COL19 = 'JPMORGAN DEPOSIT ACCT D       '))) and $PRANA_FUND_NAME != ''">
			  <PositionMaster>

        

          <xsl:choose>
            <xsl:when test="$PRANA_FUND_NAME=''">
              <AccountName>
                <xsl:value-of select="$PB_FUND_NAME"/>
              </AccountName>
            </xsl:when>
            <xsl:otherwise>
              <AccountName>
                <xsl:value-of select='$PRANA_FUND_NAME'/>
              </AccountName>
            </xsl:otherwise>
          </xsl:choose>

          <BaseCurrency>
            <xsl:value-of select="'USD'"/>
          </BaseCurrency>

			<xsl:variable name = "PB_CURRENCY_NAME" >
				<xsl:value-of select ="substring(COL1,332,2)"/>
			</xsl:variable>
			
			<xsl:variable name="PRANA_CURRENCY_NAME">
				<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name='JPM']/CurrencyData[@PBCurrencyCode=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
			</xsl:variable>

          <LocalCurrency>
			  <xsl:value-of select ="'USD'"/>
          </LocalCurrency>

          <CashValueBase>
            <xsl:value-of select="0"/>
          </CashValueBase>

				  <xsl:variable name ="varCashValLocal">
					  <xsl:choose>
						  <xsl:when test ="(COL4) = 'A' ">
							  <xsl:value-of select="number(COL42)*(-1)"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="number(COL20)"/>
						  </xsl:otherwise>
					  </xsl:choose>
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

				  <xsl:choose>
					  <xsl:when test ="boolean(number($varCashValLocal))">
						  <CashValueLocal>
							  <xsl:value-of select="$varCashValLocal"/>
						  </CashValueLocal>
					  </xsl:when >
					  <xsl:otherwise>
						  <CashValueLocal>
							  <xsl:value-of select="0"/>
						  </CashValueLocal>
					  </xsl:otherwise>
				  </xsl:choose >

          <Date>
            <xsl:value-of select="''"/>
          </Date>

          <PositionType>
            <xsl:value-of select="'Cash'"/>
          </PositionType>
				  <!-- this an optional field, whenever currency value needs to adjust, 
						set its value yes, else no or if we do not require adjustment ,need not to include this tag in the XSLT file-->
				  <DataAdjustReq>
					  <xsl:value-of select ="'yes'"/>
				  </DataAdjustReq>

        </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
