<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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
      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="COL1">
          <xsl:choose>
            <xsl:when test="substring(COL1,1,3)='USD'">
              <xsl:value-of select="concat(substring(COL1,4,3),substring(COL1,1,3))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="DealtAmt">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="ContraAmt">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="Position">
			<xsl:call-template name="Translate">
				<xsl:with-param name="Number" select="COL13"/>
			</xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and contains(COL6,'F/X') and not(contains(COL7,'U S DOLLAR'))">

          <PositionMaster>

            <xsl:variable name="varExpMon">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL10,'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(COL10,'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(COL10,'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varExpDay">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(substring-after(COL10,'/'),'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(substring-after(COL10,'/'),'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after(COL10,'/'),'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(COL10,'/'))=1">
                  <xsl:value-of select="concat('0',substring-before(COL10,'/'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(COL10,'/')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="ExpirationDate">
              <!--<xsl:value-of select="concat(substring-after(substring-after(COL10,'/'),'/'),$varExpMon,$varExpDay)"/>-->
              <xsl:value-of select="concat($varMonth,'/',$varExpDay,'/',substring(substring-after(substring-after(COL10,'/'),'/'),3))"/>
            </xsl:variable>

            <xsl:variable name="PreSymbol" select="substring($COL1,1,3)"/>
            <xsl:variable name="PostSymbol" select="substring($COL1,4)"/>
            
			  <xsl:variable name="Asset">
			  <xsl:choose>
				
				  <xsl:when test="contains(COL18,'FOREX') and normalize-space(COL6) = 'TRADE ACTIVITY'">
					  <xsl:value-of select="'FX-Spot'"/>
				  </xsl:when>
				  <xsl:when test="contains(COL6, 'F/X')">
					  <xsl:value-of select="'FX-Forward'"/>
				  </xsl:when>

				  <xsl:otherwise>
					  <xsl:value-of select="'Equity'"/>
				  </xsl:otherwise>
			  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name ="Bloomberg">
			  <xsl:choose>
				  <xsl:when test="$Asset='FX-Forward'">
					  <xsl:value-of select="concat(COL8,'/',COL3,' ','N',' ',COL10,' ','CURNCY')"/>
				  </xsl:when>
				  <xsl:when test="$Asset='FX-Spot'">
					  <xsl:value-of select="concat(COL8,COL3,' ','CURNCY')"/>
				  </xsl:when>
				 	  <xsl:otherwise>
						  <xsl:value-of select="''"/>
				  </xsl:otherwise>
			  </xsl:choose>

			  </xsl:variable>

            <TickerSymbol>
              <xsl:value-of select="$Bloomberg"/>
            </TickerSymbol>

            <AUEC>
              <xsl:value-of select="'FX-FX'"/>
            </AUEC>

            <Currency>
				<xsl:choose>
					<xsl:when test="$Asset='FX-Spot'">
						<xsl:value-of select="'USD'"/>
					</xsl:when>

					<xsl:when test="$Asset='FX-Forward'">
						<xsl:value-of select="'MUL'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
            </Currency>

            <LongName>
              <xsl:value-of select="$Bloomberg"/>
            </LongName>

            <Multiplier>
              <xsl:value-of select="1"/>
            </Multiplier>

            <UnderLyingSymbol>
              <xsl:value-of select="$Bloomberg"/>
            </UnderLyingSymbol>

            <ExpirationDate>
              <xsl:value-of select="normalize-space(COL10)"/>
            </ExpirationDate>

            <SettlementDate>
              <xsl:value-of select="normalize-space(COL10)"/>
            </SettlementDate>
			
			<FirstTradeDate>
              <xsl:value-of select="normalize-space(COL9)"/>
            </FirstTradeDate>

            <BloombergSymbol>
              <xsl:value-of select="$Bloomberg"/>
            </BloombergSymbol>

            <LeadCurrency>
              <xsl:value-of select="COL8"/>
            </LeadCurrency>

            <VsCurrency>
              <xsl:value-of select="'USD'"/>
            </VsCurrency>

            <Comments>
              <xsl:value-of select="'Created by sec master import'"/>
            </Comments>

            <SName>
              <xsl:value-of select="$Bloomberg"/>
            </SName>

            <Description>
              <xsl:value-of select="$Bloomberg"/>
            </Description>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>



