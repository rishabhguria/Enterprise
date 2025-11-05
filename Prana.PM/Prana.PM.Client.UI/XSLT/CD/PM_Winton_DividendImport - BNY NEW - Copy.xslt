<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
		  <xsl:if test="number(COL8)">
			  <!--TABLE-->
			  <PositionMaster>

				  <xsl:variable name="varPBName">
					  <xsl:value-of select="'GS'"/>
				  </xsl:variable>

				  <xsl:variable name="varSEDOL">
					  <xsl:value-of select="''"/>
				  </xsl:variable>

				  <xsl:variable name="varOSISymbol">
					  <xsl:value-of select="''"/>
				  </xsl:variable>

				  <xsl:variable name="varDividend">
					  <xsl:value-of select="COL8"/>
				  </xsl:variable>

				  <xsl:variable name="varPayoutDate">
					  <xsl:value-of select="translate(COL18, ' ','')"/>
				  </xsl:variable>

				  <xsl:variable name="varExDate">
					  <xsl:value-of select="translate(COL17, ' ','')"/>
				  </xsl:variable>

				  <!--FundNameSection -->

				  <AccountName>
					  <!--<xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME = ''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>-->
					  <xsl:value-of select="COL4"/>

				  </AccountName>

				  <PBSymbol>
					  <xsl:value-of select="COL4"/>
				  </PBSymbol>

				  <!--<SEDOL>
              <xsl:value-of select="''"/>
            </SEDOL>-->

				  <Symbol>
					  <xsl:value-of select="COL2"/>
				  </Symbol>

				  <Dividend>
					  <xsl:value-of select="number($varDividend)"/>
				  </Dividend>

				  <PayoutDate>
					  <xsl:value-of select="$varPayoutDate"/>
				  </PayoutDate>

				  <ExDate>
					  <xsl:value-of select="$varExDate"/>
				  </ExDate>

				  <Description>
					  <xsl:value-of select="'Stock Dividend'"/>
				  </Description>

			  </PositionMaster>
		  </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>