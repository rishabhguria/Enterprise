<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='JAN' and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='FEB' and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAR' and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='APR' and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAY' and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUN' and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUL' and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='AUG' and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='SEP' and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='OCT' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='NOV' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='DEC' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JAN' and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='FEB' and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAR' and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='APR' and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='MAY' and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUN' and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='JUL' and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='AUG' and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='SEP' and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='OCT' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='NOV' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='DEC' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:if test="number(COL47)">

          <PositionMaster>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= 'BNY']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:variable name="varAssetCategory" select="COL16"/>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="normalize-space(COL57)"/>
            </xsl:variable>

            <xsl:variable name="Underlier" select="COL6"/>
            <xsl:variable name="varLengthDescription" select="string-length(COL5)"/>
            <xsl:variable name="varPutOrCall" select="substring(COL5,1,1)"/>
            <xsl:variable name="varYear" select="substring(COL5,($varLengthDescription)-1,2)"/>

            <xsl:variable name="varMonth" select="substring(COL5,($varLengthDescription)-22,3)"/>
            <xsl:variable name="varStrike" select="format-number(substring(COL5,($varLengthDescription)-18,5),'#.00')"/>

            <xsl:variable name="monthCode">
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varPutCall" select="$varPutOrCall"/>
                <xsl:with-param name="varMonth" select="$varMonth"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:choose>
                <xsl:when test ="substring($varAssetCategory,1,3)='OPT'">
                  <xsl:value-of select ="concat('O:',$Underlier,' ',$varYear,$monthCode,$varStrike)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL3"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="COL6"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BNY']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>
            
            


            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test ="substring-after($varEquitySymbol, ' ') = 'US' ">
                    <Symbol>
                      <xsl:value-of select ="substring-before($varEquitySymbol, ' ')"/>
                    </Symbol>
                  </xsl:when>
                  <xsl:otherwise>
                    <Symbol>
                      <xsl:value-of select ="$varEquitySymbol"/>
                    </Symbol>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:choose>
              <xsl:when  test="boolean(number(COL48))">
                <AvgPx>
                  <xsl:value-of select="COL48"/>
                </AvgPx>
              </xsl:when >
              <xsl:otherwise>
                <AvgPx>
                  <xsl:value-of select="0"/>
                </AvgPx>
              </xsl:otherwise>
            </xsl:choose >


            <TradeDate>
              <xsl:value-of select="COL32"/>
            </TradeDate>

            <xsl:choose>
              <xsl:when  test="boolean(number(COL47))and number(COL47) &lt; 0">
                <Quantity>
                  <xsl:value-of select="COL47 * -1"/>
                </Quantity>
              </xsl:when>
              <xsl:when  test="boolean(number(COL47))and number(COL47) &gt; 0">
                <Quantity>
                  <xsl:value-of select="COL47"/>
                </Quantity>
              </xsl:when>
              <xsl:otherwise>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="number(COL67) &gt; 0">
                <Commission>
                  <xsl:value-of select="COL67"/>
                </Commission>
              </xsl:when>
				<xsl:when test="number(COL67) &lt; 0">
					<Commission>
						<xsl:value-of select="COL67*(-1)"/>
					</Commission>
				</xsl:when>
              <xsl:otherwise>
                <Commission>
                  <xsl:value-of select="0"/>
                </Commission>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL68))">
                <Fees>
                  <xsl:value-of select="number(COL68)"/>
                </Fees>
              </xsl:when>
              <xsl:otherwise>
                <Fees>
                  <xsl:value-of select="0"/>
                </Fees>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL70))">
                <MiscFees>
                  <xsl:value-of select="number(COL70)"/>
                </MiscFees>
              </xsl:when>
              <xsl:otherwise>
                <MiscFees>
                  <xsl:value-of select="0"/>
                </MiscFees>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when  test="COL20 = 'BUY'">
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
              </xsl:when>
              <xsl:when  test="COL20 = 'SELL'">
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
              </xsl:when>
            </xsl:choose>

			  <NetNotionalValue>
				  <xsl:choose>
					  <xsl:when test="number(COL49) &gt; 0">
						  <xsl:value-of select ="COL49"/>
					  </xsl:when>
					  <xsl:when test="number(COL49) &lt; 0">
						  <xsl:value-of select ="COL49*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValue>

			  <GrossNotionalValue>
				  <xsl:choose>
					  <xsl:when test="boolean(number(COL94))">
						  <xsl:value-of select ="COL94"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </GrossNotionalValue>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
