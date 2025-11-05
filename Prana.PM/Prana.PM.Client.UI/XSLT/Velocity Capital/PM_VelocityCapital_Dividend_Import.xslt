<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
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
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL12)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varDividend)">

          <PositionMaster>

            <xsl:variable name="PB_Name">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="normalize-space(COL3)"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="$PB_Symbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <Amount>
              <xsl:value-of select="$varDividend"/>
            </Amount>

            <ActivityType>
              <xsl:choose>
                <xsl:when test="$varDividend &lt; 0">
                  <xsl:value-of select="'DividendExpense'"/>
                </xsl:when>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
              </xsl:choose>
            </ActivityType>

            <Description>
              <xsl:choose>
                <xsl:when test="$varDividend &lt; 0">
                  <xsl:value-of select="'DividendExpense'"/>
                </xsl:when>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
              </xsl:choose>
            </Description>


            <xsl:variable name="varPayDate">
              <xsl:choose>
                <xsl:when test="contains(COL6,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL6),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL6,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL6,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL6,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL6),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL6,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL6,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varPayMonth">
              <xsl:choose>
                <xsl:when test="contains(COL6,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL6),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL4,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL6,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL6,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL6),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL6,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL6,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varPayYear">
              <xsl:choose>
                <xsl:when test="contains(COL6,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL6,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL6,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL6,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <PayoutDate>
              <xsl:value-of select="concat($varPayMonth,'/',$varPayDate,'/',$varPayYear)"/>
            </PayoutDate>

            <xsl:variable name="varExDate">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL4),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL4,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL4,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL4),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL4,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL4,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varExMonth">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL4),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL4,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL4,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL4),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL4,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL4,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varExYear">
              <xsl:choose>
                <xsl:when test="contains(COL4,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL4,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL4,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL4,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <ExDate>
              <xsl:value-of select="concat($varExMonth,'/',$varExDate,'/',$varExYear)"/>
            </ExDate>

            <xsl:variable name="varRecDate">
              <xsl:choose>
                <xsl:when test="contains(COL5,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL5),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL5,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL5,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL5,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL5),'-'),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(substring-after(COL5,'-'),'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(substring-after(COL5,'-'),'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varRecMonth">
              <xsl:choose>
                <xsl:when test="contains(COL5,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL5),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL5,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL5,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL5,'-')">
                      <xsl:choose>
                        <xsl:when test="string-length(substring-before(normalize-space(COL5),'-'))=1">
                          <xsl:value-of select="concat(0,substring-before(COL5,'-'))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(COL5,'-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varRecYear">
              <xsl:choose>
                <xsl:when test="contains(COL5,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL5,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="contains(COL5,'-')">
                      <xsl:value-of select="substring-after(substring-after(COL5,'-'),'-')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <RecordDate>
              <xsl:value-of select="concat($varRecMonth,'/',$varRecDate,'/',$varRecYear)"/>
            </RecordDate>

            <CurrencyName>
              <xsl:value-of select="normalize-space(COL11)"/>
            </CurrencyName>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>