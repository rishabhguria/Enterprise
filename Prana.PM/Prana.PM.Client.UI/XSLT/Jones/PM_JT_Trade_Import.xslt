<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="contains($PutOrCall,'CALL')">
      <xsl:choose>
        <xsl:when test="$Month='A' ">
          <xsl:value-of select="01"/>
        </xsl:when>
        <xsl:when test="$Month='B' ">
          <xsl:value-of select="02"/>
        </xsl:when>
        <xsl:when test="$Month='C' ">
          <xsl:value-of select="03"/>
        </xsl:when>
        <xsl:when test="$Month='D' ">
          <xsl:value-of select="04"/>
        </xsl:when>
        <xsl:when test="$Month='E' ">
          <xsl:value-of select="05"/>
        </xsl:when>
        <xsl:when test="$Month='F' ">
          <xsl:value-of select="06"/>
        </xsl:when>
        <xsl:when test="$Month='G'  ">
          <xsl:value-of select="07"/>
        </xsl:when>
        <xsl:when test="$Month='H'  ">
          <xsl:value-of select="08"/>
        </xsl:when>
        <xsl:when test="$Month='I' ">
          <xsl:value-of select="09"/>
        </xsl:when>
        <xsl:when test="$Month='J' ">
          <xsl:value-of select="10"/>
        </xsl:when>
        <xsl:when test="$Month='K' ">
          <xsl:value-of select="11"/>
        </xsl:when>
        <xsl:when test="$Month='L' ">
          <xsl:value-of select="12"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="contains($PutOrCall,'PUT')">
		<xsl:choose>
			<xsl:when test="$Month='M' ">
				<xsl:value-of select="01"/>
			</xsl:when>
			<xsl:when test="$Month='N' ">
				<xsl:value-of select="02"/>
			</xsl:when>
			<xsl:when test="$Month='O' ">
				<xsl:value-of select="03"/>
			</xsl:when>
			<xsl:when test="$Month='P' ">
				<xsl:value-of select="04"/>
			</xsl:when>
			<xsl:when test="$Month='Q' ">
				<xsl:value-of select="05"/>
			</xsl:when>
			<xsl:when test="$Month='R' ">
				<xsl:value-of select="06"/>
			</xsl:when>
			<xsl:when test="$Month='S'  ">
				<xsl:value-of select="07"/>
			</xsl:when>
			<xsl:when test="$Month='T'  ">
				<xsl:value-of select="08"/>
			</xsl:when>
			<xsl:when test="$Month='U' ">
				<xsl:value-of select="09"/>
			</xsl:when>
			<xsl:when test="$Month='V' ">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$Month='W' ">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$Month='X' ">
				<xsl:value-of select="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
		
		
     
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(COL21,'CALL') or contains(COL21,'PUT')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,' '),2,2)"/>
      </xsl:variable>
      <!--<xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,' '),3,2)"/>
      </xsl:variable>-->
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,' '),4,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,' '),6),'#.00')"/>
      </xsl:variable>


      <!--<xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>-->
		<xsl:variable name="MonthCodeVar">
			<xsl:value-of select="substring(substring-after($Symbol,' '),1,1)"/>
		</xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <!--<xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>-->
      <!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>

				<xsl:otherwise>-->
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
      <!--</xsl:otherwise>-->
      <!--

			</xsl:choose>-->
    </xsl:if>
  </xsl:template>
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'JT'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL21"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:variable name="Asset">
              <xsl:choose>



                <xsl:when test="contains(COL21,'CALL') or contains(COL21,'PUT')">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <!--<AssetType>
              <xsl:value-of select="$Asset"/>
            </AssetType>-->

            <xsl:variable name="Symbol">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <Symbol>


              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>


				  <xsl:when test="$Asset='Option'">
					  <xsl:call-template name="Option">
						  <xsl:with-param name="Symbol" select="normalize-space(COL10)"/>
						  
					  </xsl:call-template>
				  </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

            
            <xsl:variable name="PB_FUND_NAME" select="COL5"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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



            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="COL9">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9 "/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="CostBasis" select="COL12"/>



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







            <xsl:variable name="Side" select="COL8"/>

			  <SideTagValue>

				  <xsl:choose>
					  <xsl:when test="$Asset='Option'">
						  <xsl:choose>
							  <xsl:when test="$Side='B' and COL9='1'">
								  <xsl:value-of select="'A'"/>
							  </xsl:when>

							  <xsl:when test="$Side='B' and COL9='0'">
								  <xsl:value-of select="'B'"/>
							  </xsl:when>

							  <xsl:when test="$Side='S' and COL9='1'">
								  <xsl:value-of select="'C'"/>
							  </xsl:when>

							  <xsl:when test="$Side='S' and COL9='0'">
								  <xsl:value-of select="'D'"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$Side='B' and (COL9='1' or COL9='0')">
								  <xsl:value-of select="'1'"/>
							  </xsl:when>

							  <xsl:when test="$Side='S' and COL9='1'">
								  <xsl:value-of select="'5'"/>
							  </xsl:when>

							  <xsl:when test="$Side='S' and COL9='0'">
								  <xsl:value-of select="'2'"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>

					  


				  </xsl:choose>

			  </SideTagValue>


            <!--<SEDOL>
              <xsl:value-of select="COL9" />
            </SEDOL>-->

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name ="Date" select="COL3"/>


            <xsl:variable name="Year1" select="substring-after(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Month" select="substring-before(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Day" select="substring-before($Date,'/')"/>



            <PositionStartDate>

              <!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
              <xsl:value-of select="COL3"/>
            </PositionStartDate>

            <xsl:variable name ="Date1" select="COL4"/>


            <xsl:variable name="Year" select="substring-after(substring-after($Date1,'/'),'/')"/>
            <xsl:variable name="Month1" select="substring-before(substring-after($Date1,'/'),'/')"/>
            <xsl:variable name="Day1" select="substring-before($Date1,'/')"/>


            <PositionSettlementDate>
              <xsl:value-of select="$Date1"/>
              <!--<xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year)"/>-->
            </PositionSettlementDate>


            <xsl:variable name="SecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>

            <Fees>

              <xsl:choose>

                <xsl:when test="$SecFee &gt; 0">
                  <xsl:value-of select="$SecFee"/>
                </xsl:when>

                <xsl:when test="$SecFee &lt; 0">
                  <xsl:value-of select="$SecFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </Fees>


            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>

              <xsl:choose>

                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>

                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </Commission>



          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>