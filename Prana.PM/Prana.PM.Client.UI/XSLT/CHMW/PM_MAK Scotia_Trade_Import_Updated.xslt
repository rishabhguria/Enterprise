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

   
  </xsl:template>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
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
    <xsl:if test="contains(COL4,'CALL') or contains(COL4,'PUT')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,'1')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8) div 1000,'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
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
      <xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>
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
            <xsl:with-param name="Number" select="COL17"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) and (COL7='BL' or COL7='SS' or COL7='SL' or COL7='CS')">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Scotiabank'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Asset">
              <xsl:choose>



                <xsl:when test="string-length(COL27)='21'">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>
            
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
			  
           <xsl:variable name="Symbol">
				<xsl:value-of select="COL27"/>
            </xsl:variable>

            <Symbol>


              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
				  
                <xsl:when test="COL15!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL12!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL14!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>	  
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL15!='*'">
                  <xsl:value-of select="COL15"/>
                </xsl:when>
                <xsl:when test="COL12!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL14!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

			 
            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL15!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL12!='*'">
                  <xsl:value-of select="COL12"/>
                </xsl:when>
                <xsl:when test="COL14!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>


            <ISIN>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL15!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL12!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="COL14!='*'">
                  <xsl:value-of select="COL14"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISIN>

            <Symbology>
              <xsl:choose>
               <xsl:when test="$PRANA_SYMBOL_NAME !=''">
                  <xsl:value-of select="'Symbol'"/>
                </xsl:when>
                <xsl:when test="COL15!='*'">
                  <xsl:value-of select="'CUSIP'"/>
                </xsl:when>
                <xsl:when test="COL12!='*'">
                  <xsl:value-of select="'Sedol'"/>
                </xsl:when>
                <xsl:when test="COL14!='*'">
                  <xsl:value-of select="'ISIN'"/>
                </xsl:when>
				  <xsl:when test="$Symbol!='*'">
					  <xsl:value-of select="'Symbol'"/>
				  </xsl:when>
				  <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbology>

            <xsl:variable name="PB_FUND_NAME" select="COL2"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

			  <xsl:variable name="COL18">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL18"/>
				  </xsl:call-template>
			  </xsl:variable>
            <xsl:variable name="CostBasis" select="($COL18)"/>
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

            <xsl:variable name="COL28">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="translate(COL28,'-','')"/>
              </xsl:call-template>
            </xsl:variable>

			  <xsl:variable name="COL24">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL24"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name="COL19">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL19"/>
				  </xsl:call-template>
			  </xsl:variable>


			  <xsl:variable name="Bond">
				  <xsl:choose>
					  <xsl:when test="contains(COL60,'BOND') or contains(COL60,'CORP')">
						  <xsl:value-of select="(($COL19 div 100) - ($COL28  - $COL24))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
			  <xsl:variable name="Commission">
				  <xsl:choose>
					  <xsl:when test="contains(COL60,'BOND') or contains(COL60,'CORP')">
						  <xsl:value-of select="format-number($Bond,'0.##')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="format-number($COL28 - $COL19,'0.##')"/>
					  </xsl:otherwise>
				  </xsl:choose>
            
            </xsl:variable>
            <Commission>
				<xsl:choose>
					<xsl:when test="COL9='CANCEL'">
						<xsl:value-of select="$Commission * -1"/>
					</xsl:when>
					<xsl:otherwise>
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
					</xsl:otherwise>
				</xsl:choose>
             
            </Commission>

            <xsl:variable name="SecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL23"/>
              </xsl:call-template>
            </xsl:variable>
            <StampDuty>
				<xsl:choose>
					<xsl:when test="COL9='CANCEL'">
						<xsl:value-of select="$SecFee * -1"/>
					</xsl:when>
					<xsl:otherwise>
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
					</xsl:otherwise>
				</xsl:choose>
            </StampDuty>

            <xsl:variable name="Side" select="COL7"/>
            <SideTagValue>
				<xsl:choose>
					<xsl:when test="$Side='BL' and COL9='CANCEL'">
						<xsl:value-of select="'2'"/>
					</xsl:when>
					<xsl:when test="$Side='SL' and COL9='CANCEL'">
						<xsl:value-of select="'1'"/>
					</xsl:when>
					<xsl:when test="$Side='SS' and COL9='CANCEL'">
						<xsl:value-of select="'B'"/>
					</xsl:when>
					<xsl:when test="$Side='CS' and COL9='CANCEL'">
						<xsl:value-of select="'5'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$Side='BL'">
								<xsl:value-of select="'1'"/>
							</xsl:when>

							<xsl:when test="$Side='SL'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
							<xsl:when test="$Side='SS'">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="$Side='CS'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>

            </SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

			  <xsl:variable name="Month1">
				  <xsl:call-template name="MonthName">
					  <xsl:with-param name="Month" select="substring-before(substring-after(COL6,'-'),'-')"/>
				  </xsl:call-template>
			  </xsl:variable>
            <PositionStartDate>
				<xsl:choose>
					<xsl:when test="contains(substring-after(substring-after(normalize-space(COL6),'-'),'-'),'20')">
						<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL6),'-'),'/',substring-after(substring-after(COL6,'-'),'-'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL6),'-'),'/',20,substring-after(substring-after(COL6,'-'),'-'))"/>
					</xsl:otherwise>
				</xsl:choose>      
            </PositionStartDate>

			  <xsl:variable name="Month2">
				  <xsl:call-template name="MonthName">
					  <xsl:with-param name="Month" select="substring-before(substring-after(COL8,'-'),'-')"/>
				  </xsl:call-template>
			  </xsl:variable>
            <PositionSettlementDate>
				<xsl:choose>
					<xsl:when test="contains(substring-after(substring-after(normalize-space(COL8),'-'),'-'),'20')">
						<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL8),'-'),'/',substring-after(substring-after(COL8,'-'),'-'))"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL8),'-'),'/',20,substring-after(substring-after(COL8,'-'),'-'))"/>
					</xsl:otherwise>
				</xsl:choose>				
            </PositionSettlementDate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>