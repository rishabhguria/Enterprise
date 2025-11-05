<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <!--<xsl:template match="node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@*">
		<xsl:attribute name="{local-name()}" namespace="{namespace-uri()}">
			<xsl:value-of select="replace(., '^\s+|\s+$', '')"/>
		</xsl:attribute>
	</xsl:template>-->




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

  <xsl:template name="varMonth">
    <xsl:param name="MonthName"/>
    <xsl:choose>
      <xsl:when test="$MonthName='June'">
        <xsl:value-of select="6"/>
      </xsl:when>
      <xsl:when test="$MonthName='Feb'">
        <xsl:value-of select="2"/>
      </xsl:when>
      <xsl:when test="$MonthName='Mar'">
        <xsl:value-of select="3"/>
      </xsl:when>
      <xsl:when test="$MonthName='Apr'">
        <xsl:value-of select="4"/>
      </xsl:when>
      <xsl:when test="$MonthName='May'">
        <xsl:value-of select="5"/>
      </xsl:when>
      <xsl:when test="$MonthName='Jun'">
        <xsl:value-of select="6"/>
      </xsl:when>
      <xsl:when test="$MonthName='Jul'">
        <xsl:value-of select="7"/>
      </xsl:when>
      <xsl:when test="$MonthName='Aug'">
        <xsl:value-of select="8"/>
      </xsl:when>
      <xsl:when test="$MonthName='Sep'">
        <xsl:value-of select="9"/>
      </xsl:when>
      <xsl:when test="$MonthName='Oct'">
        <xsl:value-of select="10"/>
      </xsl:when>
      <xsl:when test="$MonthName='Nov'">
        <xsl:value-of select="11"/>
      </xsl:when>
      <xsl:when test="$MonthName='Dec'">
        <xsl:value-of select="12"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

  <!-- Strips trailing whitespace characters from 'string' -->
  <xsl:template name="string-rtrim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />

    <xsl:variable name="length" select="string-length($string)" />

    <xsl:if test="$length &gt; 0">
      <xsl:choose>
        <xsl:when test="contains($trim, substring($string, $length, 1))">
          <xsl:call-template name="string-rtrim">
            <xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
            <xsl:with-param name="trim"   select="$trim" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$string" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <!-- Strips leading whitespace characters from 'string' -->
  <xsl:template name="string-ltrim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />

    <xsl:if test="string-length($string) &gt; 0">
      <xsl:choose>
        <xsl:when test="contains($trim, substring($string, 1, 1))">
          <xsl:call-template name="string-ltrim">
            <xsl:with-param name="string" select="substring($string, 2)" />
            <xsl:with-param name="trim"   select="$trim" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$string" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <!-- Strips leading and trailing whitespace characters from 'string' -->
  <xsl:template name="string-trim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />
    <xsl:call-template name="string-rtrim">
      <xsl:with-param name="string">
        <xsl:call-template name="string-ltrim">
          <xsl:with-param name="string" select="$string" />
          <xsl:with-param name="trim"   select="$trim" />
        </xsl:call-template>
      </xsl:with-param>
      <xsl:with-param name="trim"   select="$trim" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="/">


    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>
        
          <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>

        <!--<xsl:variable name="varFundName" select="substring-before(substring-after(substring-after(//PositionMaster[contains(COL1,'THE DECESARIS FAMILY FNDN - LYR')]/COL1, ':'),' '),' ')"/>

        <xsl:variable name="varDate" select="substring-after(substring-before(//PositionMaster[contains(COL1,'SHTMTL20 LYRICAL')]/COL1, '-'),':')"/>-->

        <xsl:if test="number($varQuantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Fine Mark'"/>
            </xsl:variable>


            <xsl:variable name = "PB_SYMBOL_NAME">
              <xsl:value-of select ="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--<xsl:variable name="varSymbol">							
							<xsl:value-of select="substring-before(normalize-space(COL1),'$')"/>
						</xsl:variable>-->

            <xsl:variable name="varSymbol">
              <xsl:choose>
                <xsl:when test="contains(COL2,'$,^[■◆]\s*')">
                  <xsl:value-of select="translate(COL2,'$,^[■◆]\s*',' ')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL2)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <!--<xsl:variable name="varNSymbol">
              <xsl:call-template name="string-ltrim">
                <xsl:with-param name="string" select="$varSymbol"/>
              </xsl:call-template>
            </xsl:variable>-->

            <Symbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <!--<xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="normalize-space(substring($varNSymbol,3,(string-length($varSymbol) - 6)))"/>
                </xsl:when>-->

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <FundName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </FundName>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varPosition &gt; 0">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varPosition &lt; 0">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varPosition &gt; 0">
                  <xsl:value-of select="$varPosition"/>
                </xsl:when>
                <xsl:when test="$varPosition &lt; 0">
                  <xsl:value-of select="$varPosition * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>


            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="$varCostBasis"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>

                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>

                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Commission>


            <xsl:variable name="varMarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>

            <StampDuty>

              <xsl:choose>
                <xsl:when test ="$varMarketValue &lt;0">
                  <xsl:value-of select ="$varMarketValue*-1"/>
                </xsl:when>

                <xsl:when test ="$varMarketValue &gt;0">
                  <xsl:value-of select ="$varMarketValue"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>


              </xsl:choose>
            </StampDuty>

            <xsl:variable name="varMarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varMarketValueBase &gt; 0">
                  <xsl:value-of select="$varMarketValueBase"/>
                </xsl:when>
                <xsl:when test="$varMarketValueBase &lt; 0">
                  <xsl:value-of select="$varMarketValueBase * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <xsl:variable name ="MArkPrice">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <OrfFee>
              <xsl:choose>
                <xsl:when test="$MArkPrice &gt; 0">
                  <xsl:value-of select="$MArkPrice"/>
                </xsl:when>
                <xsl:when test="$MArkPrice &lt; 0">
                  <xsl:value-of select="$MArkPrice * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrfFee>

            <xsl:variable name="varUnRealized">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <TransactionLevy>
              <xsl:choose>
                <xsl:when test="$varUnRealized &gt; 0">
                  <xsl:value-of select="$varUnRealized"/>
                </xsl:when>
                <xsl:when test="$varUnRealized &lt; 0">
                  <xsl:value-of select="$varUnRealized * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionLevy>



            <!--<xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="$varMarketValueBase div $varMarketValue"/>
              </xsl:call-template>
            </xsl:variable>-->

            <xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <Strategy>
              <xsl:choose>
                <xsl:when test="$varFXRate &gt; 0">
                  <xsl:value-of select="$varFXRate"/>
                </xsl:when>
                <xsl:when test="$varFXRate &lt; 0">
                  <xsl:value-of select="$varFXRate * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Strategy>

            <xsl:variable name="varUnRealizedBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <TaxOnCommissions>
              <xsl:choose>
                <xsl:when test="number($varUnRealizedBase)">
                  <xsl:value-of select="$varUnRealizedBase"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TaxOnCommissions>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>


            <PositionStartDate>
              <xsl:value-of select="''"/>
            </PositionStartDate>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>