<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019" />
    </xsl:variable>

    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))" />
    </xsl:variable>

    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
    </xsl:variable>

    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
    </xsl:variable>

    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
    </xsl:variable>

    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))" />
    </xsl:variable>

    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
    </xsl:variable>

    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))" />
    </xsl:variable>

    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))" />
    </xsl:variable>

    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
    </xsl:variable>

    <xsl:variable name ="varMonthUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nMonth) = 1">
          <xsl:value-of select ="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="nDayUpdated">
      <xsl:choose>
        <xsl:when test ="string-length($nDay) = 1">
          <xsl:value-of select ="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="$nDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonthUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nDayUpdated"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$nYear"/>

  </xsl:template>


  <xsl:variable name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>

  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!--<xsl:template match="First|Last">
    <xsl:value-of select="concat(translate(substring(.,1,1), $vLower, $vUpper),substring(., 2),substring(' ', 1 div not(position()=last())))"/>
  </xsl:template>-->

  <!--<xsl:template match="month">
    <xsl:value-of select="concat(substring(@name, 1, 1), translate(substring(@name, 2), $uppercase, $lowercase))"/>
  </xsl:template>-->


  <xsl:template match="month">
    <xsl:value-of select="concat(substring(@name, 1, 1), translate(substring(@name, 2), $uppercase, $lowercase))"/>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL145"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varCash)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>


            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>

           <xsl:variable name="PRANA_FUND_NAME">
             <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

            <xsl:variable name="varSpace">
              <xsl:value-of select="normalize-space(COL113)"/>
            </xsl:variable>

            <xsl:variable name="varCurrencyName">
              <xsl:value-of select="$varSpace"/>
            </xsl:variable>

            <CurrencyName>
              <xsl:value-of select="$varCurrencyName"/>
            </CurrencyName>



            <xsl:variable name = "varAmount" >
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="varDescriptionName">
              <xsl:value-of select="normalize-space(COL201)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="contains(COL204,'MI') and $varCash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>

                <xsl:when test="contains(COL204,'MI') and $varCash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>

				  <xsl:when test="contains(COL204,'FNT') and $varCash &gt; 0">
					  <xsl:value-of select="'MISC_INC'"/>
				  </xsl:when>

				  <xsl:when test="contains(COL204,'FNT') and $varCash &lt; 0">
					  <xsl:value-of select="'MISC_EXP'"/>
				  </xsl:when>
				
				 <xsl:when test="contains(COL204,'MIJ') and $varCash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>

                <xsl:when test="contains(COL204,'MIJ') and $varCash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>
				
				 <xsl:when test="contains(COL204,'SBP') and $varCash &gt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>

                <xsl:when test="contains(COL204,'SBP') and $varCash &lt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>
				
					 <xsl:when test="contains(COL204,'SSJ') and $varCash &gt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>

                <xsl:when test="contains(COL204,'SSJ') and $varCash &lt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>
				
					 <xsl:when test="contains(COL204,'SSR') and $varCash &gt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>

                <xsl:when test="contains(COL204,'SSR') and $varCash &lt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>
				
					 <xsl:when test="contains(COL204,'BPJ') and $varCash &gt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>

                <xsl:when test="contains(COL204,'BPJ') and $varCash &lt; 0">
                  <xsl:value-of select="'Short_Rebate'"/>
                </xsl:when>
				<xsl:when test="contains(COL204,'FWO') and $varCash &lt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL204,'FWO') and $varCash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
				
				
				<xsl:when test="contains(COL204,'HWT') and $varCash &lt; 0">
                  <xsl:value-of select="'Dividend_Expense'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL204,'HWT') and $varCash &gt; 0">
                  <xsl:value-of select="'Dividend_Income'"/>
                </xsl:when>
				
				<xsl:when test="(contains(COL204,'ACT') and not(contains($varDescriptionName,'CUSTODY FEE')) )and $varCash &lt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="(contains(COL204,'ACT') and not(contains($varDescriptionName,'CUSTODY FEE'))) and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				
				<xsl:when test="contains($varDescriptionName,'CUSTODY FEE SEPTEMBER') and $varCash &lt; 0">
                  <xsl:value-of select="'CUST FEE'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'CUSTODY FEE') and $varCash &gt; 0">
                  <xsl:value-of select="'CUST FEE'"/>
                </xsl:when>
				
				
				<xsl:when test="contains(COL204,'FWI') and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL204,'FWI') and $varCash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
				<xsl:when test="contains(COL204,'ADC') and $varCash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
				<xsl:when test="contains(COL204,'ADC') and $varCash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
				<xsl:when test="contains($varDescriptionName,'CUST INT') and $varCash &gt; 0">
                  <xsl:value-of select="'Intincome'"/>
                </xsl:when>

                <xsl:when test="contains($varDescriptionName,'CUST INT') and $varCash &lt; 0">
                  <xsl:value-of select="'IntExpense'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'SHORTS') and $varCash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'SHORTS') and $varCash &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'SHORT')  and $varCash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'SHORT') and $varCash &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
				
				<!-- <xsl:when test="contains($varDescriptionName,'<xsl:when test="contains($varDescriptionName,'SHORT') and $varCash &lt; 0"> -->
                  <!-- <xsl:value-of select="'MISC_EXP'"/> -->
                <!-- </xsl:when> -->
				
				<!-- <xsl:when test="contains($varDescriptionName,'SHORT') and $varCash &gt; 0"> -->
                  <!-- <xsl:value-of select="'MISC_INC'"/> -->
                <!-- </xsl:when>') and $varCash &lt; 0"> -->
                  <!-- <xsl:value-of select="'MISC_EXP'"/> -->
                <!-- </xsl:when> -->
				
				<xsl:when test="contains($varDescriptionName,'SHORT') and $varCash &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
				
				<xsl:when test="(contains($varDescriptionName,'OFFSET') or contains($varDescriptionName,'CLR FEE'))and $varCash &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
				
				<xsl:when test="(contains($varDescriptionName,'CLEARING') or contains($varDescriptionName,'CLR FEE')) and $varCash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
				
				<xsl:when test="$varDescriptionName='WIRE TRANSFER CHARGE'">
                  <xsl:value-of select="'WTC'"/>
                </xsl:when>

                <xsl:when test="$varDescriptionName='CASH TRANSFER' or contains($varDescriptionName,'TRANSFER')">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'FX CHARGES') and $varCash &lt; 0">
                  <xsl:value-of select="'MISC_EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'ECN FEES') and $varCash &lt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'ECN FEES') and $varCash &gt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'REGULATORY FEES') and $varCash &lt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'REGULATORY FEES') and $varCash &gt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'OCC FEES') and $varCash &lt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'OCC FEES') and $varCash &gt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'VIRTU ECN FEES') and $varCash &lt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'VIRTU ECN FEES') and $varCash &gt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'SWP TO FUTURE') and $varCash &lt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'SWP TO FUTURE') and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'NORTHERN') and $varCash &lt; 0">
                  <xsl:value-of select="'Other_Broker_Fees'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'NORTHERN') and $varCash &gt; 0">
                  <xsl:value-of select="'Other_Broker_Fees'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'BANK OF AM-ADVENIO') and $varCash &lt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'BANK OF AM-ADVENIO') and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'MNY FROM FUTURE') and $varCash &lt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'MNY FROM FUTURE') and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'DR BANK') and $varCash &lt; 0">
                  <xsl:value-of select="'Other_Broker_Fees'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'DR BANK') and $varCash &gt; 0">
                  <xsl:value-of select="'Other_Broker_Fees'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'JPMORGAN') and $varCash &lt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				
				<xsl:when test="contains($varDescriptionName,'JPMORGAN') and $varCash &gt; 0">
                  <xsl:value-of select="'OTH EXP'"/>
                </xsl:when>
				<xsl:when test="contains($varDescriptionName,'SEC FEE REBATE') and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				<xsl:when test="contains($varDescriptionName,'REV NRA RET') and $varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				<xsl:when test="contains(COL204,'AAT')">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>
				

                <!--<xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="'CashTransferIn'"/>
                </xsl:when>-->
                

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="concat('Cash:', $varAmount , '|', $PRANA_ACRONYM_NAME, ':' , $varAmount)"/>
                </xsl:when>

                <xsl:when  test="$varCash &lt; 0">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME,':' , $varAmount , '|Cash:' , $varAmount)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

            <xsl:variable name="varDate">
              <xsl:value-of select="concat(substring(COL73,5,2),'/',substring(COL73,7,2),'/',substring(COL73,1,4))"/>
            </xsl:variable>

            <Date>
              <xsl:value-of select="$varDate"/>
            </Date>


           

            <xsl:variable name="varName">
              <xsl:value-of select="concat(substring($varDescriptionName, 1, 1), translate(substring($varDescriptionName, 2), $uppercase, $lowercase))"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select="$varDescriptionName"/>
            </Description>



          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>