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
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
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


  <xsl:template name="MonthCode1">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth=1">
        <xsl:value-of select="'F'"/>
      </xsl:when>
      <xsl:when test="$varMonth=2">
        <xsl:value-of select="'G'"/>
      </xsl:when>
      <xsl:when test="$varMonth=3">
        <xsl:value-of select="'H'"/>
      </xsl:when>
      <xsl:when test="$varMonth=4">
        <xsl:value-of select="'J'"/>
      </xsl:when>
      <xsl:when test="$varMonth=5">
        <xsl:value-of select="'K'"/>
      </xsl:when>
      <xsl:when test="$varMonth=6">
        <xsl:value-of select="'M'"/>
      </xsl:when>
      <xsl:when test="$varMonth=7">
        <xsl:value-of select="'N'"/>
      </xsl:when>
      <xsl:when test="$varMonth=8">
        <xsl:value-of select="'Q'"/>
      </xsl:when>
      <xsl:when test="$varMonth=9">
        <xsl:value-of select="'U'"/>
      </xsl:when>
      <xsl:when test="$varMonth=10">
        <xsl:value-of select="'V'"/>
      </xsl:when>
      <xsl:when test="$varMonth=11">
        <xsl:value-of select="'X'"/>
      </xsl:when>
      <xsl:when test="$varMonth=12">
        <xsl:value-of select="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="normalize-space(COL6)='Options'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before($Symbol,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after($Symbol,' '),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after($Symbol,' '),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after($Symbol,' '),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after($Symbol,' '),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after($Symbol,' '),8) div 1000,'#.00')"/>
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




  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL22"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and contains(COL6,'Cash')!='true'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'WELLS FARGO'"/>
            </xsl:variable>

			  <xsl:variable name="PB_FUND_NAME" select="COL9"/>

			  <xsl:variable name = "PB_SYMBOL_NAME" >
				  <xsl:value-of select ="COL13"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_FUND_DATA">
				  <xsl:value-of select="document('../ReconMappingXml/FundSymbolDescriptionMapping.xml')/FundSymbolMapping/PB[@Name=$PB_NAME]/FundSymbolData[@SymbolDescription=$PB_SYMBOL_NAME and @FundDescription=$PB_FUND_NAME]/@Fund"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_DATA">
				  <xsl:value-of select="document('../ReconMappingXml/FundSymbolDescriptionMapping.xml')/FundSymbolMapping/PB[@Name=$PB_NAME]/FundSymbolData[@SymbolDescription=$PB_SYMBOL_NAME]/@Symbol"/>
			  </xsl:variable>

			  <xsl:variable name="PB_SUFFIX_NAME">

				  <xsl:choose>
					  <xsl:when test="contains(COL27,'_')">
						  <xsl:value-of select="substring-before(COL27,'_')"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="COL27"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SUFFIX_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
			  </xsl:variable>

			  <xsl:variable name="AssetType">
				  <xsl:choose>

					  <xsl:when test="contains(COL6,'Options')">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>

					  <xsl:when test="contains(COL6,'Fixed Income')">
						  <xsl:value-of select="'Fixed Income'"/>
					  </xsl:when>

					  <xsl:when test="contains(COL6,'Futures')">
						  <xsl:value-of select="'Future'"/>
					  </xsl:when>

					  <xsl:when test="contains(COL6,'Private Placements')">
						  <xsl:value-of select="'Private'"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="PB_ROOT_NAME">

				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL12,'_'))=5">
						  <xsl:value-of select="concat(substring(COL12,1,3),' ',substring(COL12,4,2))"/>
					  </xsl:when>

					  <xsl:when test="string-length(substring-before(COL12,'_'))=4">
						  <xsl:value-of select="concat(substring(COL12,1,2),' ',substring(COL12,3,2))"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="substring-before(COL12,'_')"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="PB_YELLOW_NAME">
				  <!--<xsl:value-of select="normalize-space()"/>-->
				  <!--<xsl:choose>
								<xsl:when test ="contains(substring-after(normalize-space(COL20),' '),' ')">
									<xsl:value-of select="substring-after(substring-after(normalize-space(COL20),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(normalize-space(COL20),' ')"/>
								</xsl:otherwise>
							</xsl:choose>-->
				  <xsl:value-of select="normalize-space(COL41)"/>
			  </xsl:variable>

			  <!--<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL91)"/>
						</xsl:variable>-->

			  <xsl:variable name ="PRANA_ROOT_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @YellowFlag = $PB_YELLOW_NAME]/@UnderlyingCode"/>
			  </xsl:variable>

			  <xsl:variable name ="FUTURE_EXCHANGE_CODE">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
			  </xsl:variable>

			  <xsl:variable  name="FUTURE_FLAG">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExpFlag"/>
			  </xsl:variable>

			  <xsl:variable name="MonthCode">
				  <!--<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="number(substring-before(substring-after(normalize-space(COL18),'/'),'/'))"/>
							</xsl:call-template>-->
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL12,'_'))=5">
						  <xsl:value-of select="substring(COL12,4,1)"/>
					  </xsl:when>

					  <xsl:when test="string-length(substring-before(COL12,'_'))=4">
						  <xsl:value-of select="substring(COL12,3,1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="Year" >

				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL12,'_'))=5">
						  <xsl:value-of select="substring(COL12,5,1)"/>
					  </xsl:when>

					  <xsl:when test="string-length(substring-before(COL12,'_'))=4">
						  <xsl:value-of select="substring(COL12,4,1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </xsl:variable>

			  <xsl:variable name="MonthYearCode">
				  <xsl:choose>
					  <xsl:when test="$FUTURE_FLAG!=''">
						  <xsl:value-of select="concat($Year,$MonthCode)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="concat($MonthCode,$Year)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="Underlying">
				  <xsl:choose>
					  <xsl:when test="$PRANA_ROOT_NAME!=''">
						  <xsl:value-of select="translate($PRANA_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="translate($PB_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>





			  <xsl:variable name="Symbol" select="COL12"/>

			  <Symbol>

				  <xsl:choose>

					  <xsl:when test="$PRANA_SYMBOL_DATA!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_DATA"/>
					  </xsl:when>

					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>

					  <xsl:when test="$AssetType='Private'">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>

					  <xsl:when test="normalize-space(COL6)='Options'">
						  <xsl:call-template name="Option">
							  <xsl:with-param name="Symbol" select="normalize-space(COL12)"/>
						  </xsl:call-template>

					  </xsl:when>

					  <xsl:when test="$AssetType='Future'">
						  <xsl:value-of select="$PB_ROOT_NAME"/>
					  </xsl:when>

					  <xsl:when test="$AssetType='Equity'">
						  <xsl:value-of select="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
					  </xsl:when>



					  <!--<xsl:when test="$AssetType='Equity'">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>-->

					  <xsl:otherwise>
						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </Symbol>

			  <!--<IDCOOptionSymbol>
				  <xsl:choose>
					  -->
			  <!--<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>-->
			  <!--
					  <xsl:when test="contains(COL6,'Option')">
						  <xsl:value-of select="concat(COL12,'U')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>-->

           

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
				  
				  
				    <xsl:when test ="$PRANA_FUND_DATA!=''">
                  <xsl:value-of select ="$PRANA_FUND_DATA"/>
                </xsl:when>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
				  
				  

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </AccountName>

            <Quantity>
				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<xsl:value-of select="$Quantity"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
			</Quantity>

			<xsl:variable name="Side" select="COL4"/>


			<Side>

				<xsl:choose>

                <xsl:when test="$AssetType='Equity'">
                  <xsl:choose>


                    <xsl:when test="$Side='L'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>

                    <xsl:when test="$Side='S'">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>

                       <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>
                </xsl:when>

                <xsl:when test="$AssetType='Option'">
                  <xsl:choose>


                    <xsl:when test="$Side='L'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>

                    <xsl:when test="$Side='S'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>

                       <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>
                </xsl:when>

              </xsl:choose>
            </Side>

       


            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL38"/>
              </xsl:call-template>
            </xsl:variable>


            <MarkPrice>
              <xsl:choose>

                <xsl:when test="$MarkPrice &gt; 0">
                  <xsl:value-of select="$MarkPrice"/>
                </xsl:when>

                <xsl:when test="$MarkPrice &lt; 0">
                  <xsl:value-of select="$MarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>


            </MarkPrice>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL34"/>
              </xsl:call-template>
            </xsl:variable>


            <AvgPX>
              <xsl:choose>

                <xsl:when test="$AvgPrice &gt; 0">
                  <xsl:value-of select="$AvgPrice"/>
                </xsl:when>

                <xsl:when test="$AvgPrice &lt; 0">
                  <xsl:value-of select="$AvgPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>


            </AvgPX>


            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL29"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValue>

              <xsl:choose>
                <xsl:when test="$Side='L'">

                  <xsl:choose>
                    <xsl:when test="$NetNotionalValue &gt; 0">
                      <xsl:value-of select="$NetNotionalValue"/>
                    </xsl:when>

                    <xsl:when test="$NetNotionalValue &lt; 0">
                      <xsl:value-of select="$NetNotionalValue *(-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>



                  </xsl:choose>
                </xsl:when>

                <xsl:when test="$Side='S'">
                  <xsl:choose>
                    <xsl:when test="$NetNotionalValue &gt; 0">
                      <xsl:value-of select="$NetNotionalValue*(-1)"/>
                    </xsl:when>

                    <xsl:when test="$NetNotionalValue &lt; 0">
                      <xsl:value-of select="$NetNotionalValue "/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
                
              </xsl:choose>
             


            </NetNotionalValue>



            <xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL31"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValueBase>

              <xsl:choose>
                <xsl:when test="$Side='L'">

                  <xsl:choose>
                    <xsl:when test="$NetNotionalValueBase &gt; 0">
                      <xsl:value-of select="$NetNotionalValueBase"/>
                    </xsl:when>

                    <xsl:when test="$NetNotionalValueBase &lt; 0">
                      <xsl:value-of select="$NetNotionalValueBase *(-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>



                  </xsl:choose>
                </xsl:when>

                <xsl:when test="$Side='S'">
                  <xsl:choose>
                    <xsl:when test="$NetNotionalValueBase &gt; 0">
                      <xsl:value-of select="$NetNotionalValueBase*(-1)"/>
                    </xsl:when>

                    <xsl:when test="$NetNotionalValueBase &lt; 0">
                      <xsl:value-of select="$NetNotionalValueBase "/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

              </xsl:choose>



            </NetNotionalValueBase>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL46"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>

              <xsl:choose>
                <xsl:when test="$Side='L'">

                  <xsl:choose>
                    <xsl:when test="$MarketValue &gt; 0">
                      <xsl:value-of select="$MarketValue"/>
                    </xsl:when>

                    <xsl:when test="$MarketValue &lt; 0">
                      <xsl:value-of select="$MarketValue *(-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>



                  </xsl:choose>
                </xsl:when>

                <xsl:when test="$Side='S'">
                  <xsl:choose>
                    <xsl:when test="$MarketValue &gt; 0">
                      <xsl:value-of select="$MarketValue*(-1)"/>
                    </xsl:when>

                    <xsl:when test="$MarketValue &lt; 0">
                      <xsl:value-of select="$MarketValue "/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

              </xsl:choose>
            </MarketValue>

            <xsl:variable name="MarketValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL47"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test="$Side='L'">

                  <xsl:choose>
                    <xsl:when test="$MarketValueBase &gt; 0">
                      <xsl:value-of select="$MarketValueBase"/>
                    </xsl:when>

                    <xsl:when test="$MarketValueBase &lt; 0">
                      <xsl:value-of select="$MarketValueBase *(-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>



                  </xsl:choose>
                </xsl:when>

                <xsl:when test="$Side='S'">
                  <xsl:choose>
                    <xsl:when test="$MarketValueBase &gt; 0">
                      <xsl:value-of select="$MarketValueBase*(-1)"/>
                    </xsl:when>

                    <xsl:when test="$MarketValueBase &lt; 0">
                      <xsl:value-of select="$MarketValueBase "/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>

              </xsl:choose>
            </MarketValueBase>





            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name ="Date" select="COL1"/>


            <!--<xsl:variable name="Year1" select="substring-after(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Month" select="substring-before(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Day" select="substring-before($Date,'/')"/>-->



            <TradeDate>

              <!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
              <xsl:value-of select="$Date"/>
            </TradeDate>

			  <CurrencySymbol>
				  <xsl:value-of select="COL27"/>
			  </CurrencySymbol>

			  <ISINSymbol>
				  <xsl:value-of select="COL107"/>
			  </ISINSymbol>

			  <SEDOLSymbol>
				  <xsl:value-of select="COL106"/>
			  </SEDOLSymbol>

			  <CUSIPSymbol>
				  <xsl:value-of select="COL94"/>
			  </CUSIPSymbol>
			  <CompanyName>
				  <xsl:value-of select="translate(COL13,$lower_CONST,$upper_CONST)"/>
			  </CompanyName>

			  <Asset>
				  <xsl:choose>
					  <xsl:when test="$AssetType='Option'">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL6,'Right')">
						  <xsl:value-of select="'PrivateEquity'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL6,'Fixed')">
						  <xsl:value-of select="'FixedIncome'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
					  
			  </Asset>

            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>


          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>